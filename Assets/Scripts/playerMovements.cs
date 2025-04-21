using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;
    private BoxCollider2D myCapCollider2D;
    private Animator myAnimator;
    [SerializeField] public float runSpeed = 10f;
    [SerializeField] public float jumpSpeed = 10f;
    [SerializeField] private AudioSource meowAudioSource;
    public AudioClip swipe;
    [SerializeField] private AudioClip[] meowClips;
    [SerializeField] private Transform hurtBox;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    float startingGravityScale;
    [SerializeField] private float climbSpeed = 5f;
    private bool isClimbing = false;

    private bool isCrouching = false;

    public bool IsMeowing => isMeowing;
    private bool isMeowing = false;

    [SerializeField] private CapsuleCollider2D crouchCollider;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private float ceilingCheckRadius = 0.2f;
    [SerializeField] private LayerMask ceilingLayer;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;

    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myCapCollider2D = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        startingGravityScale = myRigidBody2D.gravityScale;

        if (crouchCollider != null) crouchCollider.enabled = false;
    }

    void Update()
    {
        Crouch(); //  Evaluate crouch status first

        Climb();  //  Updates isClimbing properly now

        if (!isClimbing && !isCrouching)
        {
            Run();

            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
            }
        }
        else if (!isClimbing && isCrouching)
        {
            Run();
        }

        if (!isCrouching)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E) && !isMeowing)
        {
            Meow();
        }
    }

    private void Climb()
    {
        bool touchingClimb = myCapCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (touchingClimb)
        {
            isClimbing = true;

            // Get player input for both axes
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            Vector2 climbingVelocity = new Vector2(horizontalInput * runSpeed, verticalInput * climbSpeed);
            myRigidBody2D.linearVelocity = climbingVelocity;

            myAnimator.SetBool("Climbing", true);
            myRigidBody2D.gravityScale = 0f;

            myAnimator.speed = Mathf.Approximately(verticalInput, 0f) && Mathf.Approximately(horizontalInput, 0f) ? 0f : 1f;
        }
        else
        {
            isClimbing = false;

            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = startingGravityScale;
            myAnimator.speed = 1f;
        }
    }

    private void Jump()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded) return;

        if (Input.GetButtonDown("Jump"))
        {
            myRigidBody2D.linearVelocity = new Vector2(myRigidBody2D.linearVelocity.x, jumpSpeed);
        }
    }

    private void Run()
    {
        float controlThrow = Input.GetAxisRaw("Horizontal");

        if (isCrouching)
        {
            float crouchSpeed = runSpeed * crouchSpeedMultiplier;
            Vector2 crouchVelocity = new Vector2(controlThrow * crouchSpeed, myRigidBody2D.linearVelocity.y);
            myRigidBody2D.linearVelocity = crouchVelocity;

            bool isMovingCrouched = Mathf.Abs(controlThrow) > Mathf.Epsilon;
            myAnimator.speed = isMovingCrouched ? 1f : 0f;

            FlipSprite(); // Allow flipping while crouching
            return;
        }

        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.linearVelocity.y);
        myRigidBody2D.linearVelocity = playerVelocity;

        FlipSprite();
        ChangingToRunningState();

        void ChangingToRunningState()
        {
            bool runningHorizontally = Mathf.Abs(myRigidBody2D.linearVelocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", runningHorizontally);
        }
    }

    private void FlipSprite()
    {
        bool runningHorizontally = Mathf.Abs(myRigidBody2D.linearVelocity.x) > Mathf.Epsilon;

        if (runningHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.linearVelocity.x), 1f);
        }
    }

    public void Meow()
    {
        if (meowClips.Length == 0 || meowAudioSource == null)
            return;

        AudioClip selectedClip;

        if (Random.value <= .05f && meowClips.Length > 1)
        {
            selectedClip = meowClips[0]; // Rare meow
        }
        else
        {
            int randomIndex = Random.Range(1, meowClips.Length);
            selectedClip = meowClips[randomIndex];
        }

        meowAudioSource.PlayOneShot(selectedClip);
        StartCoroutine(WaitUntilMeowEnds(selectedClip.length));
    }

    private System.Collections.IEnumerator WaitUntilMeowEnds(float duration)
    {
        isMeowing = true;
        yield return new WaitForSeconds(duration);
        isMeowing = false;
    }

    public void Attack()
    {
        /**f (isClimbing)
        {
            myAnimator.ResetTrigger("Attacking"); // ensure it's not stuck
            return;
        }**/

        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return; // Prevent spamming attacks

        myAnimator.SetTrigger("Attacking");

        if (swipe != null)
            meowAudioSource.PlayOneShot(swipe);

        Collider2D[] leversToHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Lever"));

        foreach (Collider2D lever in leversToHit)
        {
            Lever leverScript = lever.GetComponent<Lever>();
            if (leverScript != null)
            {
                leverScript.FlipSprite();
            }
        }

        Collider2D[] breakables = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Breakable"));

        foreach (Collider2D trash in breakables)
        {
            TrashcanInteraction trashcanScript = trash.GetComponent<TrashcanInteraction>();
            if (trashcanScript != null)
            {
                trashcanScript.TakeHit();
            }
        }

    }



    private void Crouch()
    {
        bool hasCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, ceilingLayer);

        if (isClimbing)
        {
            ResetCrouch();
            return;
        }

        // Hold crouch if the player pressed Left Shift or there's a ceiling
        bool wantsToCrouch = Input.GetKey(KeyCode.LeftShift) || hasCeiling;

        isCrouching = wantsToCrouch;

        myCapCollider2D.enabled = !isCrouching;
        if (crouchCollider != null)
            crouchCollider.enabled = isCrouching;

        myAnimator.SetBool("Crouching", isCrouching);
    }

    private void ResetCrouch()
    {
        isCrouching = false;

        myCapCollider2D.enabled = true;
        if (crouchCollider != null)
            crouchCollider.enabled = false;

        myAnimator.SetBool("Crouching", false);
        myAnimator.speed = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        if (hurtBox != null)
        {
            Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (ceilingCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }

    }
}
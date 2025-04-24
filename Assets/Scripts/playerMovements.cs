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
        if (Time.timeScale == 0f) return;

        Crouch();
        Climb();

        if (!isClimbing && !isCrouching)
        {
            Run();
            if (Input.GetButtonDown("Fire1"))
                Attack();
        }
        else if (!isClimbing && isCrouching)
        {
            Run();
        }

        if (!isCrouching)
            Jump();

        if (Input.GetKeyDown(KeyCode.E) && !isMeowing)
            Meow();
    }

    private void Climb()
    {
        bool touchingClimb = myCapCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        if (touchingClimb)
        {
            isClimbing = true;
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxisRaw("Horizontal");
            myRigidBody2D.linearVelocity = new Vector2(h * runSpeed, v * climbSpeed);
            myAnimator.SetBool("Climbing", true);
            myRigidBody2D.gravityScale = 0f;
            myAnimator.speed = (Mathf.Approximately(v,0f) && Mathf.Approximately(h,0f)) ? 0f : 1f;
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
            myRigidBody2D.linearVelocity = new Vector2(myRigidBody2D.linearVelocity.x, jumpSpeed);
    }

    private void Run()
    {
        float controlThrow = Input.GetAxisRaw("Horizontal");
        if (isCrouching)
        {
            float cs = runSpeed * crouchSpeedMultiplier;
            myRigidBody2D.linearVelocity = new Vector2(controlThrow * cs, myRigidBody2D.linearVelocity.y);
            myAnimator.speed = Mathf.Abs(controlThrow) > Mathf.Epsilon ? 1f : 0f;
            FlipSprite();
            return;
        }
        myRigidBody2D.linearVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.linearVelocity.y);
        FlipSprite();
        myAnimator.SetBool("Running", Mathf.Abs(myRigidBody2D.linearVelocity.x) > Mathf.Epsilon);
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(myRigidBody2D.linearVelocity.x) > Mathf.Epsilon)
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.linearVelocity.x), 1f);
    }

    public void Meow()
    {
        if (meowClips.Length == 0 || meowAudioSource == null) return;
        AudioClip clip = (Random.value <= .05f && meowClips.Length > 1)
            ? meowClips[0]
            : meowClips[Random.Range(1, meowClips.Length)];
        meowAudioSource.PlayOneShot(clip);
        StartCoroutine(WaitUntilMeowEnds(clip.length));
    }

    private System.Collections.IEnumerator WaitUntilMeowEnds(float duration)
    {
        isMeowing = true;
        yield return new WaitForSeconds(duration);
        isMeowing = false;
    }

    public void Attack()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;
        myAnimator.SetTrigger("Attacking");
        if (swipe != null) meowAudioSource.PlayOneShot(swipe);

        foreach (var lever in Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Lever")))
            if (lever.TryGetComponent<Lever>(out var ls)) ls.FlipSprite();

        foreach (var b in Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Breakable")))
            if (b.TryGetComponent<TrashcanInteraction>(out var ts)) ts.TakeHit();
    }

    private void Crouch()
    {
        if (isClimbing) { ResetCrouch(); return; }
        bool hasCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, ceilingLayer);
        bool wants = Input.GetKey(KeyCode.LeftShift) || hasCeiling;
        isCrouching = wants;
        myCapCollider2D.enabled = !wants;
        if (crouchCollider != null) crouchCollider.enabled = wants;
        myAnimator.SetBool("Crouching", wants);
    }

    private void ResetCrouch()
    {
        isCrouching = false;
        myCapCollider2D.enabled = true;
        if (crouchCollider != null) crouchCollider.enabled = false;
        myAnimator.SetBool("Crouching", false);
        myAnimator.speed = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        if (hurtBox != null) Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
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
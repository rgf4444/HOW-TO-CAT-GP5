using UnityEngine;
public class RatBehavior : MonoBehaviour
{
    public float speed = 6f;
    public GameObject conditionObject;
    private BoxCollider2D triggerCollider;

    private Transform player;
    private bool isFollowing = false;
    private Rigidbody2D myRigidBody2D;

    private void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Enable the trigger collider once the condition object is destroyed
        if (conditionObject == null && triggerCollider != null && !triggerCollider.enabled)
        {
            triggerCollider.enabled = true;
        }

        if (isFollowing && player != null)
        {
            float playerInputX = Input.GetAxisRaw("Horizontal");

            if (playerInputX != 0)
            {
                myRigidBody2D.linearVelocity = new Vector2(playerInputX * speed, myRigidBody2D.linearVelocity.y);
                FlipSprite();
            }
            else
            {
                myRigidBody2D.linearVelocity = new Vector2(0, myRigidBody2D.linearVelocity.y);
            }
        }
        else
        {
            myRigidBody2D.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowing = false;
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
}

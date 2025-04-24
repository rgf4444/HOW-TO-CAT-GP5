using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RatBehavior : MonoBehaviour
{
    public Transform player; // Assign in Inspector
    public float followSpeed = 2f; // Speed of rat movement
    public float stopDistance = 0.1f; // Distance threshold before stopping
    public bool playerInRange = false;
    public bool canFollow = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (playerInRange && canFollow)
        {
            float direction = player.position.x - transform.position.x;

            // Move only if the rat isn't very close to the player
            if (Mathf.Abs(direction) > stopDistance)
            {
                float moveDir = Mathf.Sign(direction); // +1 or -1
                rb.linearVelocity = new Vector2(moveDir * followSpeed, rb.linearVelocity.y);

                // Flip sprite based on direction
                spriteRenderer.flipX = moveDir < 0;
            }
            else
            {
                // Stop horizontal movement when close
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            // If not following, maintain vertical velocity (e.g., gravity)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void AllowFollow()
    {
        canFollow = true;
    }
}

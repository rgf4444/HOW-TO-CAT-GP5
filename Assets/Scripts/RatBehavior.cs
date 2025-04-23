using UnityEngine;
public class RatBehavior : MonoBehaviour
{
    public Transform player; // Assign this in the Inspector or dynamically
    public bool playerInRange = false;
    public bool canFollow = false;

    private SpriteRenderer spriteRenderer;
    private float previousXPosition; // To store previous position for movement direction

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousXPosition = transform.position.x; // Initial x position
    }

    void Update()
    {
        if (playerInRange && canFollow)
        {
            // Follow player on x-axis
            Vector3 newPosition = transform.position;
            newPosition.x = player.position.x;
            transform.position = newPosition;

            // Check movement direction and flip sprite accordingly
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        // Compare current x position with the previous one to determine movement direction
        if (transform.position.x > previousXPosition)
        {
            // Moving to the right (positive x direction)
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x < previousXPosition)
        {
            // Moving to the left (negative x direction)
            spriteRenderer.flipX = true;
        }

        // Update the previous x position for the next frame
        previousXPosition = transform.position.x;
    }

    public void AllowFollow()
    {
        canFollow = true;
    }
}

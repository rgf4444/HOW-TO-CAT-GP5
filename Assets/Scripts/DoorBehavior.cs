using UnityEngine;

public class DoorBehavior: MonoBehaviour
{
    [SerializeField] private Vector2 teleportLocation;
    [SerializeField] private Transform player;
    private Rigidbody2D playerRb;

    private void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player)
        {
            player.position = teleportLocation;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
    }
}

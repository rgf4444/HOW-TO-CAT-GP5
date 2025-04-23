using UnityEngine;

public class RatTrigger : MonoBehaviour
{
    [SerializeField] private GameObject conditionObject; // The object that needs to be destroyed
    private RatBehavior ratFollow;

    private void Start()
    {
        ratFollow = GetComponentInParent<RatBehavior>();
    }

    private void Update()
    {
        // Check if the condition object is destroyed (null) or inactive
        if (conditionObject == null)
        {
            // Activate follow behavior once the condition is met
            ratFollow.AllowFollow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ratFollow.playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ratFollow.playerInRange = false;
        }
    }
}

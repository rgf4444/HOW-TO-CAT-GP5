using UnityEngine;

public class RatPit : MonoBehaviour
{
    [SerializeField] private GameObject objectToEnable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat"))
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }
        }
    }
}

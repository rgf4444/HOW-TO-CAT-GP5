using UnityEngine;

public class TrashcanInteraction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int hitsToBreak = 3;
    [SerializeField] private GameObject hiddenObject;

    private int currentHits = 0;
    private bool isBroken = false;

    private Animator trashAnimator;

    void Start()
    {
        trashAnimator = GetComponent<Animator>();
    }

    public void TakeHit()
    {
        if (isBroken) return;

        currentHits++;

        if (trashAnimator != null)
        {
            trashAnimator.SetTrigger("Hit");
        }

        if (currentHits >= hitsToBreak)
        {
            Break();
        }
    }

    private void Break()
    {
        isBroken = true;

        if (trashAnimator != null)
        {
            trashAnimator.SetTrigger("Break");
        }

        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true);
        }

        // Destroy the entire trashcan GameObject after a short delay (to let the break animation play)
        Destroy(gameObject, 1f);
    }
}

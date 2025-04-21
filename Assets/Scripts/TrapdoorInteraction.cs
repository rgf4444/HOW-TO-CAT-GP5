using UnityEngine;
using System.Collections; 

public class TrapdoorInteraction : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D trapdoorCollider;
    private CapsuleCollider2D triggerCollider;
    [SerializeField] private float rearmDuration = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        trapdoorCollider = GetComponent<BoxCollider2D>();
        triggerCollider = GetComponent<CapsuleCollider2D>();

        SetTrapdoorState(0); // Start in standby
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTrapdoorState(1); // Active
            StartCoroutine(StartRearming());
        }
    }

    private IEnumerator StartRearming()
    {
        yield return new WaitForSeconds(.75f); // Delay before opening
        trapdoorCollider.enabled = false;
        triggerCollider.enabled = false;
        SetTrapdoorState(2); // Rearming (open trapdoor)
        yield return new WaitForSeconds(rearmDuration); // Duration of rearming animation
        trapdoorCollider.enabled = true;
        triggerCollider.enabled = true;
        SetTrapdoorState(0); // Back to standby
    }

    private void SetTrapdoorState(int state)
    {
        animator.SetInteger("State", state);
    }
}

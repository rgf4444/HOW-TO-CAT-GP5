using UnityEngine;
using System.Collections;
public class CatTreatBehavior : MonoBehaviour
{
    public float boostMultiplier = 2f;    // Multiplier for speed and jump
    public float boostDuration = 20f;     // Duration of the boost

    private Coroutine boostCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            if (player != null)
            {
                if (boostCoroutine != null)
                {
                    StopCoroutine(boostCoroutine);
                }

                boostCoroutine = StartCoroutine(ApplyBoost(player));
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ApplyBoost(PlayerMovements player)
    {

        float originalSpeed = player.runSpeed;
        float originalJumpForce = player.jumpSpeed;

        player.runSpeed *= boostMultiplier;
        player.jumpSpeed *= boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        player.runSpeed = originalSpeed;
        player.jumpSpeed = originalJumpForce;
    }
}

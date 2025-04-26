using UnityEngine;

public class CatTreatBehavior : MonoBehaviour
{
    [SerializeField] private float bonusAmount = 7f;
    [SerializeField] private float buffDuration = 20f;  

    private Collider2D col;
    private SpriteRenderer sr;
    private bool isBuffActive = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            if (player != null && !isBuffActive)
            {
                StartCoroutine(ApplyTemporaryBuff(player));
            }

            col.enabled = false;
            sr.enabled = false;
        }
    }

    private System.Collections.IEnumerator ApplyTemporaryBuff(PlayerMovements player)
    {
        isBuffActive = true;

        player.runSpeed += bonusAmount;
        player.jumpSpeed += bonusAmount;

        yield return new WaitForSeconds(buffDuration);

        player.runSpeed -= bonusAmount;
        player.jumpSpeed -= bonusAmount;

        col.enabled = true;
        sr.enabled = true;
        isBuffActive = false;
    }
}



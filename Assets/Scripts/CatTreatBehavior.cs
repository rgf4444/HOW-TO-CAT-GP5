using UnityEngine;

public class CatTreatBehavior : MonoBehaviour
{
    public float speedBuff = 7f;
    public float buffDuration = 20f;
    public GameObject indicatorImage;
    public GameObject catTreatPrefab; // Assign the prefab manually in the inspector

    private static bool isBuffActive = false;
    private static float originalRunSpeed;
    private static float originalJumpSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBuffActive)
        {
            PlayerMovements player = collision.GetComponent<PlayerMovements>();
            if (player != null)
            {
                originalRunSpeed = player.runSpeed;
                originalJumpSpeed = player.jumpSpeed;

                player.runSpeed += speedBuff;
                player.jumpSpeed += speedBuff;

                isBuffActive = true;
                if (indicatorImage != null) indicatorImage.SetActive(true);

                player.StartCoroutine(RemoveBuffAfterDuration(player));
                CatTreatSpawner.instance.SpawnCatTreat(transform.position, 5f);

                Destroy(gameObject);
            }
        }
    }

    private System.Collections.IEnumerator RemoveBuffAfterDuration(PlayerMovements player)
    {
        yield return new WaitForSeconds(buffDuration);
        player.runSpeed = originalRunSpeed;
        player.jumpSpeed = originalJumpSpeed;
        if (indicatorImage != null) indicatorImage.SetActive(false);
        isBuffActive = false;
    }

    private System.Collections.IEnumerator RespawnCatTreat(Vector3 position)
    {
        yield return new WaitForSeconds(5f);
        Instantiate(catTreatPrefab, position, Quaternion.identity);
    }
}


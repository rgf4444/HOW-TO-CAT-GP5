using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTriggerController : MonoBehaviour
{
    [Tooltip("Scene index to load when player enters the trigger.")]
    public int sceneIndexToLoad = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}

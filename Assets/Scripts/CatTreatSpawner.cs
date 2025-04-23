using UnityEngine;

public class CatTreatSpawner : MonoBehaviour
{
    public static CatTreatSpawner instance;
    public GameObject catTreatPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnCatTreat(Vector3 position, float delay)
    {
        StartCoroutine(SpawnAfterDelay(position, delay));
    }

    private System.Collections.IEnumerator SpawnAfterDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(catTreatPrefab, position, Quaternion.identity);
    }
}

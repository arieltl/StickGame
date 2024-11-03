using UnityEngine;
using System.Collections;

public class TimedAreaSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnArea
    {
        public Transform areaTransform;         // Reference to the spawn area GameObject
        public Vector2 areaSize;                // Width and height of the spawn area
        public float spawnInterval = 2f;        // Individual spawn interval for each area
        public int maxObjectsInArea = 5;        // Maximum objects allowed in this area
    }

    public GameObject[] prefabsToSpawn;         // Array of prefabs to spawn
    public SpawnArea[] spawnAreas;              // Array of spawn areas

    private void Start()
    {
        // Start a separate coroutine for each spawn area
        foreach (SpawnArea spawnArea in spawnAreas)
        {
            StartCoroutine(SpawnObjectsInArea(spawnArea));
        }
    }

    private IEnumerator SpawnObjectsInArea(SpawnArea spawnArea)
    {
        while (true)
        {
            // Spawn an object in this specific area
            SpawnInArea(spawnArea);

            // Wait for this area's specific interval before spawning the next object
            yield return new WaitForSeconds(spawnArea.spawnInterval);
        }
    }

    private void SpawnInArea(SpawnArea spawnArea)
    {
        // Choose a random prefab from the array
        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

        // Generate a random position within the spawn area's bounds
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnArea.areaTransform.position.x - spawnArea.areaSize.x / 2, spawnArea.areaTransform.position.x + spawnArea.areaSize.x / 2),
            Random.Range(spawnArea.areaTransform.position.y - spawnArea.areaSize.y / 2, spawnArea.areaTransform.position.y + spawnArea.areaSize.y / 2),
            0f // Assuming a 2D game; set Z to 0
        );

        // Spawn the prefab at the random position
        Instantiate(prefab, randomPosition, Quaternion.identity);
    }
}

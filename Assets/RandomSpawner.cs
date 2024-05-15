using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] cubePrefabs; // Array of cube prefabs
    private int spawnCount = 0;

    void Update()
    {
        if (spawnCount < 300)
        {
            // Get the position of the spawner
            Vector3 spawnerPosition = transform.position;

            // Calculate random X, Y, and Z offsets
            float randomXOffset = Random.Range(-80f, 80f);
            float randomYOffset = Random.Range(-15f, 10f);
            float randomZOffset = Random.Range(-80f, 80f);

            // Combine the random offsets with the spawner's position
            Vector3 randomSpawnPosition = new Vector3(spawnerPosition.x + randomXOffset, spawnerPosition.y + randomYOffset, spawnerPosition.z + randomZOffset);

            // Randomly select a cube prefab from the array
            GameObject selectedPrefab = cubePrefabs[Random.Range(0, cubePrefabs.Length)];

            // Instantiate the selected cube prefab at the calculated spawn position
            Instantiate(selectedPrefab, randomSpawnPosition, Quaternion.identity);

            // Increment the spawn count
            spawnCount++;
        }
    }
}


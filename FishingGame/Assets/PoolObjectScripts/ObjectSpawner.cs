using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] goodObjects; // Array of good objects prefabs
    public GameObject[] badObjects;  // Array of bad objects prefabs

    public Transform[] spawnPoints;  // Array of 3 spawn points for each row
    public float spawnInterval = 2f; // Time between spawning waves
    public float minSpawnDelay = 0.5f; // Minimum delay between spawns in the same row
    public float maxSpawnDelay = 1.5f; // Maximum delay between spawns in the same row

    private float screenRightEdge; // Right edge of the screen for spawning
    private Queue<int> availableRows; // Queue to keep track of available rows

    void Start()
    {
        // Calculate the right edge of the screen
        screenRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Initialize the queue of available rows
        availableRows = new Queue<int>(new[] { 0, 1, 2 });

        // Start the spawning loop
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Randomize the order of rows
            List<int> rowList = new List<int>(availableRows);
            rowList.Shuffle(); // Custom extension method to shuffle the list

            foreach (int rowIndex in rowList)
            {
                // Choose whether to spawn a good or bad object
                GameObject prefabToSpawn;
                if (Random.value < 0.5f)
                {
                    prefabToSpawn = goodObjects[Random.Range(0, goodObjects.Length)];
                }
                else
                {
                    prefabToSpawn = badObjects[Random.Range(0, badObjects.Length)];
                }

                // Instantiate the object at the current spawn point
                Instantiate(prefabToSpawn, new Vector3(screenRightEdge, spawnPoints[rowIndex].position.y, 0), Quaternion.identity);

                // Wait for a random delay before moving to the next row
                yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            }

            // Wait before starting the next spawning wave
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

// Extension method to shuffle a list
public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

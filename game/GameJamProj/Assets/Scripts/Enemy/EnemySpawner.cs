/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script spawns enemies randomly around the map
**/

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Variables needed for this script
    public float xRange;
    public float yRange;

    public float spawnRate;
    public float spawnInterval;
    public float startTime;

    [SerializeField] private GameObject[] enemyObjects;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemies), startTime, spawnInterval);
    }

    public void SpawnEnemies()
    {
        for(int num = 0; num < spawnRate; num++)
        {
            Vector2 randomPosition = new(transform.position.x + Random.Range(-xRange, xRange), transform.position.y + Random.Range(-yRange, yRange));
            GameObject enemyObject = enemyObjects[Random.Range(0, enemyObjects.Length)];
            Instantiate(enemyObject, randomPosition, Quaternion.identity);
        }
    }
}
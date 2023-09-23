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
    [SerializeField] private GameObject enemyObject;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemies), 3.0f, spawnInterval);
    }

    private void SpawnEnemies()
    {
        for(int num = 0; num < spawnRate; num++)
        {
            Vector2 randomPosition = new(transform.position.x + Random.Range(-xRange, xRange), transform.position.y + Random.Range(-yRange, yRange));
            Instantiate(enemyObject, randomPosition, Quaternion.identity);
        }
    }
}
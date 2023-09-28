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
    public float xTransformPosition;
    public float yTransformPosition;

    public float spawnRate;
    public float spawnInterval;
    public float startTime;

    [SerializeField] private GameObject[] enemyObjects;
    [SerializeField] private GameObject enemyProjectile;

    private void Start()
    {
        xTransformPosition = transform.position.x;
        yTransformPosition = transform.position.y;

        InvokeRepeating(nameof(SpawnEnemies), startTime, spawnInterval);
    }

    public void SpawnEnemies()
    {
        for (int num = 0; num < spawnRate; num++)
        {
            Vector2 randomPosition = new(xTransformPosition + Random.Range(-xRange, xRange), yTransformPosition + Random.Range(-yRange, yRange));
            GameObject enemyObject = enemyObjects[Random.Range(0, enemyObjects.Length)];
            Instantiate(enemyObject, randomPosition, Quaternion.identity);
        }

        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        Vector2 randomPosition = new(xTransformPosition + Random.Range(-xRange, xRange), yTransformPosition + Random.Range(-yRange, yRange));
        Instantiate(enemyProjectile, randomPosition, Quaternion.identity);
    }
}
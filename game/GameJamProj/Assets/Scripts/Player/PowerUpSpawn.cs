/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script spawns power ups randomly around the map
**/

using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawn : MonoBehaviour
{
    // Variables needed for this script
    public float xRange;
    public float yRange;
    public float maximumPowerups;

    [SerializeField] private GameObject[] powerUps;
    private readonly List<GameObject> currentPowerUps = new();

    private void Start()
    {
        SpawnPowerUps();
    }

    public void SpawnPowerUps()
    {
        for(int p = 0; p < maximumPowerups; p++)
        {
            int randomIndex = Random.Range(0, powerUps.Length);
            GameObject selectedPowerUp = powerUps[randomIndex];

            Vector3 spawnPosition = new(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), 0f);

            GameObject newPowerUp = Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);

            currentPowerUps.Add(newPowerUp);
        }
    }

    public void ClearPowerUps()
    {
        foreach (GameObject powerUp in currentPowerUps)
        {
            Destroy(powerUp);
        }
        currentPowerUps.Clear();
    }
}
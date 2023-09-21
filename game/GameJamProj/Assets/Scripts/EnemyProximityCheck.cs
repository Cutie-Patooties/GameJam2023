using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityCheck : MonoBehaviour
{
    // Variables needed for this script
    public int numberOfEnemies = 0;

    // This will keep track of how many enemies are within a radius
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemies++;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemies--;
        }
    }

    private void Update()
    {
        Debug.Log("Number of Enemies: " + numberOfEnemies);
    }
}

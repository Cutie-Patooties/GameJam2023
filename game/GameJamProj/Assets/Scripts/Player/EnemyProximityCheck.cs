/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script keeps track of how many enemies are near the player
**/

using UnityEngine;

public class EnemyProximityCheck : MonoBehaviour
{
    // Variables needed for this script
    public int numberOfEnemies = 0;
    public ParticleSystem proximityEffect;

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

    // This will emit particles based on how many enemies are nearby
    private void Update()
    {
        var emission = proximityEffect.emission;

        if (numberOfEnemies >= 10)
        {
            emission.rateOverTime = 50f; // Rate when there are 10 or more enemies
        }
        else if (numberOfEnemies >= 5)
        {
            emission.rateOverTime = 25f; // Rate when there are 5 or more enemies
        }
        else if (numberOfEnemies >= 1)
        {
            emission.rateOverTime = 5f; // Default rate when fewer enemies are nearby
        }
        else
        {
            emission.rateOverTime = 0f; // When there are no enemies nearby
        }
    }
}
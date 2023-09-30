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
    public SpriteRenderer proximityEffect;
    private Color originalColor;

    private void Awake()
    {
        originalColor = proximityEffect.color;
        Color updatedColor = originalColor;
        updatedColor.a = 0.05f;
        proximityEffect.color = updatedColor;
    }

    // This will keep track of how many enemies are within a radius
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemies++;
            UpdateAlpha();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemies--;
            UpdateAlpha();
        }
    }

    // This will give visual indication on how many enemies are around them
    private void UpdateAlpha()
    {
        if (numberOfEnemies >= 10)
        {
            Color updatedColor = originalColor;
            updatedColor.a = 0.55f;
            proximityEffect.color = updatedColor;
        }
        else if (numberOfEnemies >= 5)
        {
            Color updatedColor = originalColor;
            updatedColor.a = 0.35f;
            proximityEffect.color = updatedColor;
        }
        else if (numberOfEnemies >= 1)
        {
            Color updatedColor = originalColor;
            updatedColor.a = 0.15f;
            proximityEffect.color = updatedColor;
        }
        else
        {
            Color updatedColor = originalColor;
            updatedColor.a = 0.05f;
            proximityEffect.color = updatedColor;
        }
    }
}
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
    private readonly float lowAlpha = 0.5f;
    private readonly float highAlpha = 1.0f;

    private void Awake()
    {
        originalColor = proximityEffect.color;
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
        float alpha = Mathf.Lerp(lowAlpha, highAlpha, numberOfEnemies / 10.0f);
        alpha = Mathf.Clamp(alpha, lowAlpha, highAlpha);

        Color updatedColor = originalColor;
        updatedColor.a = alpha;
        proximityEffect.color = updatedColor;
    }
}
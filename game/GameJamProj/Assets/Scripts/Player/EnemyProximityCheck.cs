/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script keeps track of how many enemies are near the player
**/

using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class EnemyProximityCheck : MonoBehaviour
{
    // Variables needed for this script
    public int numberOfEnemies = 0;
    public SpriteRenderer proximityEffect;
    private Color originalColor;
    private float fadeTime = 5.0f;

    private void Awake()
    {
        originalColor = proximityEffect.color;
        Color updatedColor = originalColor;
        updatedColor.a = 0.05f;
        proximityEffect.color = updatedColor;
    }

    public void Update()
    {
        UpdateAlpha();
    }

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

    // This will give visual indication on how many enemies are around them
    private void UpdateAlpha()
    {
        if (numberOfEnemies >= 10)
        {
            Color updatedColor = originalColor;
            updatedColor.a = Mathf.Lerp(updatedColor.a, 0.65f, Time.deltaTime * fadeTime);
            proximityEffect.color = updatedColor;
        }
        else if (numberOfEnemies >= 5)
        {
            Color updatedColor = originalColor;
            updatedColor.a = Mathf.Lerp(updatedColor.a, 0.45f, Time.deltaTime * fadeTime);
            proximityEffect.color = updatedColor;
        }
        else if (numberOfEnemies >= 1)
        {
            Color updatedColor = originalColor;
            updatedColor.a = Mathf.Lerp(updatedColor.a, 0.25f, Time.deltaTime * fadeTime);
            proximityEffect.color = updatedColor;
        }
        else
        {
            Color updatedColor = originalColor;
            updatedColor.a = Mathf.Lerp(updatedColor.a, 0.05f, Time.deltaTime * fadeTime);
            proximityEffect.color = updatedColor;
        }
    }
}
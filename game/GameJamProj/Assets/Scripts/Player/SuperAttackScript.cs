/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles the logic for the player activating their super attack
**/

using UnityEngine;

public class SuperAttackScript : MonoBehaviour
{
    // Variables needed for this script
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private float shakeIntensity;

    // This will kill all enemies upon hitting them
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnEnable()
    {
        explosionEffect.Play();
        Camera.main.GetComponent<CameraShake>().Shake(0.5f, shakeIntensity);
    }
}
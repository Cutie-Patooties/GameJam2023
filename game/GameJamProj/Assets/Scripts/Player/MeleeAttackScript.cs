/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles the logic for the player hitting an enemy with a melee attack
**/

using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    // Variables needed for this script
    [SerializeField] private ParticleSystem killEffect;
    [SerializeField] private float shakeIntensity;

    // This will kill an enemy upon hitting them
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            killEffect.Play();
            Camera.main.GetComponent<CameraShake>().Shake(0.1f, shakeIntensity);
        }
    }
}
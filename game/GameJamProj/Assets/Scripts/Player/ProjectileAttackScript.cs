/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles the logic for the player shooting a projectile
**/

using UnityEngine;

public class ProjectileAttackScript : MonoBehaviour
{
    // Variables needed for this script
    public int damage = 1;
    [SerializeField] private GameObject blastEffect;

    // This will handle logic for when projectile collides with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Replace this code later with dealing damage
            Instantiate(blastEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script handles the logic for the player shooting a projectile
**/

using UnityEngine;

public class ProjectileAttackScript : MonoBehaviour
{
    // Variables needed for this script
    public int damage = 1;
    public bool isPlayer;

    [SerializeField] private GameObject blastEffect;

    [SerializeField] private float maxAliveTime = 5.0f;

    // Keep track of time object has existed for
    private float aliveTime = 0.0f;

    void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > maxAliveTime)
            Destroy(this.gameObject);
    }

    // This will handle logic for when projectile collides with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isPlayer)
        {
            other.GetComponent<EnemyController>().maxHealth -= damage;
            Instantiate(blastEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.CompareTag("Miniboss") && isPlayer)
        {
            other.GetComponent<MinibossScript>().maxHealth -= damage;
            Instantiate(blastEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player") && !isPlayer)
        {
            other.GetComponent<PlayerController>().currentHealth -= damage;
            Instantiate(blastEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
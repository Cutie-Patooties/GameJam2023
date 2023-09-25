/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script makes enemies chase down the player
**/

using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variables needed for this script

    public float movementSpeed;
    public float maxHealth;

    public float attackCooldown;
    public float attackDamage;
    private bool canAttack;

    private Rigidbody2D enemyrb;
    private GameObject player;

    private void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("EntityPlayer");
        canAttack = true;
    }

    private void Update()
    {
        // Have enemy go towards player's position
        Vector2 direction = (player.transform.position - transform.position).normalized;
        enemyrb.velocity = direction * movementSpeed;

        if (maxHealth <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Damage player by certain amount and grant invincibility to this enemy
        if (other.CompareTag("Player") && canAttack)
        {
            canAttack = false;
            other.GetComponent<PlayerController>().currentHealth -= attackDamage;
            StartCoroutine(EnableAttack());
        }
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
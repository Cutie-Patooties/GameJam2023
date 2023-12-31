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
    private float currentHealth;
    private Color originalColor;

    public float attackCooldown;
    public float attackDamage;
    private bool canAttack;

    private Rigidbody2D enemyrb;
    private SpriteRenderer enemySprite;
    private GameObject player;
    private GameManager game;

    private void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("EntityPlayer");
        game = GameObject.Find("GameManager").GetComponent<GameManager>();

        originalColor = enemySprite.color;

        canAttack = true;
        maxHealth += (game.wave - 1);
    }

    private void Update()
    {
        // Have enemy go towards player's position
        Vector2 direction = (player.transform.position - transform.position).normalized;
        enemyrb.velocity = direction * movementSpeed;

        // Enemy dies when health reaches zero or wave ends
        if (maxHealth <= 0)
        {
            game.score += 10;
            Destroy(gameObject);
        }
        else if (game.hasEnded)
            Destroy(gameObject);

        // Flash RED to indicate enemy has gotten hit

        if (maxHealth < currentHealth)
        {
            StartCoroutine(FlashRed());
        }
        currentHealth = maxHealth;
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

    IEnumerator FlashRed()
    {
        enemySprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = originalColor;
    }
}
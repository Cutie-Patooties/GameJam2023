/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles the behavior of the mini boss
**/

using System.Collections;
using UnityEngine;

public class MinibossScript : MonoBehaviour
{
    // Variables needed for this script

    // Miniboss chases player slowly, killing miniboss kills all remaining enemies
    public float movementSpeed;
    public float maxHealth;
    private float currentHealth;
    private Color originalColor;
    private SpriteRenderer enemySprite;
    [SerializeField] private GameObject minibossKillEffect;

    // Miniboss hurts player same way regular enemies do
    public float attackCooldown;
    public float attackDamage;
    private bool canAttack;

    // Miniboss can shoot a large, slow moving projectile
    public float projectileCooldown;
    public float projectileSpeed;
    [SerializeField] private GameObject minibossProjectile;

    // Other variables needed
    private Rigidbody2D enemyrb;
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
        InvokeRepeating(nameof(LaunchProjectile), projectileCooldown, projectileCooldown);
    }

    private void Update()
    {
        // Have enemy go towards player's position
        Vector2 direction = (player.transform.position - transform.position).normalized;
        enemyrb.velocity = direction * movementSpeed;

        // Enemy dies when health reaches zero or wave ends
        if (maxHealth <= 0)
        {
            Instantiate(minibossKillEffect, gameObject.transform.position, Quaternion.identity);
            game.PrepareNextWave();
            game.bossAppeared = false;
            game.score += 1000;
            Destroy(gameObject);
        }

        // Flash RED to indicate enemy has gotten hit

        if (maxHealth < currentHealth)
        {
            StartCoroutine(FlashRed());
        }
        currentHealth = maxHealth;
    }

    private void LaunchProjectile()
    {
        GameObject projectile = Instantiate(minibossProjectile, transform.position, Quaternion.identity);

        // Correctly get direction projectile will travel at
        Vector2 direction = (player.transform.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction;
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<Rigidbody2D>().velocity.normalized * projectileSpeed;
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
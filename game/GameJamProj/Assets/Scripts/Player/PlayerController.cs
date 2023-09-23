/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles both player movement and combat
**/

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables regarding current player information
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float invincibilityDuration = 3f;
    private bool isInvincible;

    // Variables needed for Movement
    public float movementSpeed = 1f;
    private Rigidbody2D playerRigidBody;
    private Vector2 lastMovementDirection;

    // Variables needed for Attacking
    public float attackDelay = 0.1f;
    private bool canAttack;
    [SerializeField] private GameObject attackHitbox;

    // Variables needed for Shooting
    public float shootDelay = 0.1f;
    public float projectileSpeed = 1;
    [SerializeField] private GameObject projectileObject;

    // Receive and Set all Necessary Components
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        canAttack = true;
        isInvincible = false;
    }

    private void Update()
    {
        // Logic for implementing Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movementInput = new(horizontalInput, verticalInput);
        playerRigidBody.velocity = movementSpeed * movementInput;

        if (movementInput != Vector2.zero)
            lastMovementDirection = movementInput.normalized;

        // This is for properly positioning the hitbox
        if (canAttack)
            if (movementInput != Vector2.zero)
            {
                Vector2 hitboxOffset = lastMovementDirection * 1.45f;
                attackHitbox.transform.position = (Vector2)transform.position + hitboxOffset;
            }

        // Logic for performing Attack
        if (Input.GetButtonDown("Attack") && canAttack)
        {
            canAttack = false;
            attackHitbox.SetActive(true);
            StartCoroutine(EnableAttack());
        }

        // Logic for shooting Projectile
        if (Input.GetButton("Shoot") && canAttack)
        {
            canAttack = false;
            GameObject projectile = Instantiate(projectileObject, attackHitbox.transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = lastMovementDirection * projectileSpeed;
            StartCoroutine(EnableProjectile());
        }

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackHitbox.SetActive(false);
        canAttack = true;
    }

    IEnumerator EnableProjectile()
    {
        yield return new WaitForSeconds(shootDelay);
        canAttack = true;
    }

    IEnumerator EnableInvincibility()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && !isInvincible)
        {
            isInvincible = true;
            currentHealth -= 10;
            StartCoroutine(EnableInvincibility());
        }
    }
}
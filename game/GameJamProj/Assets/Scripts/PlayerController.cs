/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles both player movement and combat
**/

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables needed for Movement
    public float movementSpeed = 1f;
    private Rigidbody2D playerRigidBody;
    private Vector2 lastMovementDirection;

    // Variables needed for Attacking
    public float attackDelay = 0.1f;
    private bool canAttack;
    [SerializeField] private GameObject attackHitbox;

    // Receive and Set all Necessary Components
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        canAttack = true;
    }

    private void Update()
    {
        // Logic for implementing Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movementInput = new(horizontalInput, verticalInput);
        playerRigidBody.velocity = movementInput * movementSpeed;

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
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackHitbox.SetActive(false);
        canAttack = true;
    }
}
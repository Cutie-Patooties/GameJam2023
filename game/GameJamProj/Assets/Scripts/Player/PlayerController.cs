/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script handles player movement and stats
**/

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables regarding current player information
    public float maxHealth = 100;
    public float currentHealth = 100;

    // Variables needed for Movement
    public float movementSpeed = 1f;
    private Vector2 lastMovementDirection;
    private Rigidbody2D playerRigidBody;
    [SerializeField] private GameObject attackHitbox;

    // Receive and Set all Necessary Components
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
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
        if (movementInput != Vector2.zero)
        {
            Vector2 hitboxOffset = lastMovementDirection * 1.25f;
            attackHitbox.transform.position = (Vector2)transform.position + hitboxOffset;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died.");
        }
    }

    private void OnDisable()
    {
        playerRigidBody.velocity = new(0, 0);
    }
}
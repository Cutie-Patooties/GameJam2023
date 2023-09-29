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
    public Vector2 lastMovementDirection;
    private Rigidbody2D playerRigidBody;
    private Animator playerAnimation;
    private string currentTrigger = "";

    // Receive and Set all Necessary Components
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        // Logic for implementing Movement

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementInput = new(horizontalInput, verticalInput);
        playerRigidBody.velocity = movementSpeed * movementInput;

        if (movementInput != Vector2.zero)
        {
            lastMovementDirection = movementInput.normalized;

            if (Mathf.Abs(lastMovementDirection.x) > Mathf.Abs(lastMovementDirection.y))
            {
                if (lastMovementDirection.x > 0 && currentTrigger != "MoveRight")
                {
                    playerAnimation.SetTrigger("MoveRight");
                    currentTrigger = "MoveRight";
                }
                else if (lastMovementDirection.x < 0 && currentTrigger != "MoveLeft")
                {
                    playerAnimation.SetTrigger("MoveLeft");
                    currentTrigger = "MoveLeft";
                }
            }
            else
            {
                if (lastMovementDirection.y > 0 && currentTrigger != "MoveUp")
                {
                    playerAnimation.SetTrigger("MoveUp");
                    currentTrigger = "MoveUp";
                }
                else if (lastMovementDirection.y < 0 && currentTrigger != "MoveDown")
                {
                    playerAnimation.SetTrigger("MoveDown");
                    currentTrigger = "MoveDown";
                }
            }
        }
    }

    private void OnDisable()
    {
        playerRigidBody.velocity = new(0, 0);
    }
}
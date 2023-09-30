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
    private RangedWeaponManager playerWeapons;
    private Rigidbody2D playerRigidBody;
    private Animator playerAnimation;
    private string currentTrigger = "";

    // Receive and Set all Necessary Components
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerWeapons = GetComponent<RangedWeaponManager>();
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
            playerAnimation.speed = 1.0f;

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
        else
        {
            playerAnimation.speed = 0.0f;
        }
    }

    private void OnDisable()
    {
        playerRigidBody.velocity = new(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("RapidPowerUp"))
        {
            playerWeapons.AddWeapon(new RangedWeapon("Rapid Fire", Color.white, playerWeapons.defaultWeaponSprite, null, 2, 0.1f, 5, 60.0f, 3.5f, playerWeapons.projectileObject));
            Destroy(other.gameObject);
        }
        if(other.CompareTag("BurstPowerUp"))
        {
            playerWeapons.AddWeapon(new WeaponShotgun("Burst Shot", Color.white, playerWeapons.shotgunSprite, null, 2, 0.3f, 10, 25.0f, 1f, playerWeapons.projectileObject, Mathf.Deg2Rad * 10.0f));
            Destroy(other.gameObject);
        }
    }
}
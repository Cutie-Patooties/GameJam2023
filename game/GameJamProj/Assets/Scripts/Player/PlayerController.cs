/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script handles player movement and stats
**/

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject debugManager = null;

    // Variables regarding current player health
    public float maxHealth = 100;
    public float currentHealth = 100;
    private float previousHealth;
    private SpriteRenderer playerSprite;
    private Color originalColor;

    // Other Variables needed
    public float movementSpeed = 1f;
    public Vector2 lastMovementDirection;

    private RangedWeaponManager playerWeapons;
    private Rigidbody2D playerRigidBody;

    private Animator playerAnimation;
    private string currentTrigger = "";

    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private AudioSource hurtSound;

    // Receive and Set all Necessary Components
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerWeapons = GetComponent<RangedWeaponManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        originalColor = playerSprite.color;

        // Attempt to find debug manager if it is null
        if (debugManager == null) debugManager = GameObject.Find("DebugInfo");
    }

    private void Update()
    {
        // Give coordinates to debug manager
        if (debugManager != null)
        {
            debugManager.GetComponent<DebugManager>().UpdateCustomDebugField(
                "player_coords", "XYZ: " + transform.position.x + " / " + transform.position.y + " / " + transform.position.z
            );
        }

        // Logic for implementing Movement

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementInput = new(horizontalInput, verticalInput);
        playerRigidBody.velocity = movementSpeed * movementInput;

        // Logic for setting animation based on movement direction

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

        // Flash RED to indicate player has gotten hit

        if (currentHealth < previousHealth)
        {
            StartCoroutine(FlashRed());
        }
        previousHealth = currentHealth;
    }

    private void OnDisable()
    {
        playerRigidBody.velocity = new(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("RapidPowerUp"))
        {
            playerWeapons.AddWeapon(new RangedWeapon("Rapid Fire", Color.magenta, playerWeapons.defaultWeaponSprite, playerWeapons.defaultWeaponSound, 2, 0.1f, 5, 60.0f, 3.5f, playerWeapons.projectileObject));
            pickupSound.PlayOneShot(pickupSound.clip);
            Destroy(other.gameObject);
        }
        if(other.CompareTag("BurstPowerUp"))
        {
            playerWeapons.AddWeapon(new WeaponShotgun("Burst Shot", Color.magenta, playerWeapons.shotgunSprite, playerWeapons.shotgunSound, 2, 0.3f, 10, 25.0f, 1f, playerWeapons.projectileObject, Mathf.Deg2Rad * 10.0f));
            pickupSound.PlayOneShot(pickupSound.clip);
            Destroy(other.gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        hurtSound.PlayOneShot(hurtSound.clip);

        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        playerSprite.color = originalColor;
    }
}
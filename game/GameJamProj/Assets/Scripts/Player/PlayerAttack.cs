/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script handles player input for melee and super attacks
**/

using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    // Variables needed for this script
    [System.NonSerialized] public bool canAttack;
    private PlayerController playerController;
    private BoxCollider2D playerHitbox;
    private Animator playerAnimator;
    private Camera playerCamera;

    // Variables needed for Attacking
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private AudioSource attackSound;
    public float attackDelay = 0.1f;
    public float hitboxOffset = 1.25f;

    // Variables needed for Shooting
    public int damageIncrease = 0;
    public TextMeshProUGUI damageText;
    private EnemyProximityCheck enemyCheck;

    // Variables needed for Super Attack
    [SerializeField] private GameObject superRadius;
    [SerializeField] private GenericBar superMeter;
    public float superDuration = 0.5f;
    public float energyRate = 0.5f;
    private float currentEnergy = 0.0f;
    private float animationStartup = 0.5f;
    private bool canUnleashUltimateDestruction = false;

    private void Start()
    {
        playerCamera = Camera.main;
        playerController = GetComponent<PlayerController>();
        playerHitbox = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
        enemyCheck = GetComponentInChildren<EnemyProximityCheck>();

        canAttack = true;
        damageText.text = damageIncrease.ToString("D2");
    }

    private void Update()
    {
        // Logic for performing Attack
        if (Input.GetButtonDown("Attack - Mouse") && canAttack)
        {
            canAttack = false;

            // Correctly places hitbox for melee attack
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -playerCamera.transform.position.z;
            Vector2 worldMousePosition = playerCamera.ScreenToWorldPoint(mousePosition);
            Vector2 hitboxOffsetPosition = (worldMousePosition - (Vector2)transform.position).normalized * hitboxOffset;
            attackHitbox.transform.position = (Vector2)transform.position + hitboxOffsetPosition;

            attackHitbox.SetActive(true);
            attackSound.PlayOneShot(attackSound.clip);
            StartCoroutine(EnableAttack());
        }
        if (Input.GetButtonDown("Attack - Shift") && canAttack)
        {
            canAttack = false;

            // Correctly places hitbox for melee attack
            Vector2 hitboxOffsetPosition = playerController.lastMovementDirection * hitboxOffset;
            attackHitbox.transform.position = (Vector2)transform.position + hitboxOffsetPosition;

            attackHitbox.SetActive(true);
            attackSound.PlayOneShot(attackSound.clip);
            StartCoroutine(EnableAttack());
        }

        // Logic for increasing projectile damage based on number of enemies nearby
        damageIncrease = 0;
        damageIncrease += enemyCheck.numberOfEnemies;
        damageText.text = damageIncrease.ToString("D2");

        // Logic for increasing the super meter by a certain rate
        if (currentEnergy < 100)
            currentEnergy += (energyRate * (enemyCheck.numberOfEnemies + 1)) * Time.deltaTime;
        if (currentEnergy > 100)
            currentEnergy = 100;
        if (currentEnergy == 100)
            canUnleashUltimateDestruction = true;
        superMeter.SetBarValue(currentEnergy);

        // Logic for performing a super attack
        if (Input.GetButtonDown("Super") && canUnleashUltimateDestruction)
        {
            StartCoroutine(ActivateSuper());
        }
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackHitbox.SetActive(false);
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    IEnumerator ActivateSuper()
    {
        canAttack = false;
        canUnleashUltimateDestruction = false;

        playerController.enabled = false;
        playerHitbox.enabled = false;

        playerAnimator.SetTrigger("Special");
        yield return new WaitForSeconds(animationStartup);

        superRadius.SetActive(true);
        superRadius.GetComponent<SuperAttackScript>().enabled = true;
        superRadius.GetComponent<AudioSource>().Play();

        currentEnergy = 0;

        yield return new WaitForSeconds(superDuration);

        playerAnimator.SetTrigger("MoveDown");

        superRadius.SetActive(false);
        superRadius.GetComponent<SuperAttackScript>().enabled = false;

        canAttack = true;
        playerController.enabled = true;

        yield return new WaitForSeconds(0.5f);

        playerHitbox.enabled = true;
    }
}
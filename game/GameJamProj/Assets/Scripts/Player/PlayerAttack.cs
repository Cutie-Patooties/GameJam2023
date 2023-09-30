/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script handles player input for melee and super attacks
**/

using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Variables needed for this script
    [System.NonSerialized] public bool canAttack;
    private PlayerController playerController;

    // Variables needed for Attacking
    [SerializeField] private GameObject attackHitbox;
    public float attackDelay = 0.1f;
    public float hitboxOffset = 1.25f;

    // Variables needed for Shooting
    public GameObject projectileObject;
    public int damageIncrease = 0;
    private Camera playerCamera;
    private EnemyProximityCheck enemyCheck;

    // Variables needed for Super Attack
    [SerializeField] private GameObject superRadius;
    [SerializeField] private GenericBar superMeter;
    public float superDuration = 0.5f;
    public float energyRate = 0.5f;
    private float currentEnergy = 0.0f;
    private bool canUnleashUltimateDestruction = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        enemyCheck = GetComponentInChildren<EnemyProximityCheck>();
        playerCamera = Camera.main;
        canAttack = true;
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
            StartCoroutine(EnableAttack());
        }
        if (Input.GetButtonDown("Attack - Shift") && canAttack)
        {
            canAttack = false;

            // Correctly places hitbox for melee attack
            Vector2 hitboxOffsetPosition = playerController.lastMovementDirection * hitboxOffset;
            attackHitbox.transform.position = (Vector2)transform.position + hitboxOffsetPosition;

            attackHitbox.SetActive(true);
            StartCoroutine(EnableAttack());
        }

        // Logic for increasing projectile damage based on number of enemies nearby
        damageIncrease = 0;
        damageIncrease += enemyCheck.numberOfEnemies;

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
            canAttack = false;
            canUnleashUltimateDestruction = false;

            superRadius.SetActive(true);
            superRadius.GetComponent<SuperAttackScript>().enabled = true;
            playerController.enabled = false;

            currentEnergy = 0;
            StartCoroutine(ResetSuper());
        }
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackHitbox.SetActive(false);
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    IEnumerator ResetSuper()
    {
        yield return new WaitForSeconds(superDuration);

        superRadius.SetActive(false);
        superRadius.GetComponent<SuperAttackScript>().enabled = false;

        canAttack = true;
        playerController.enabled = true;
    }
}
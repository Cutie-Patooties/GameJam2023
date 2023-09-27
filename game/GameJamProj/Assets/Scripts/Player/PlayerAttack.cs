/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script handles player combat, melee and ranged
**/

using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Variables needed for this script
    private bool canAttack;
    private PlayerController playerController;

    // Variables needed for Attacking
    [SerializeField] private GameObject attackHitbox;
    public float attackDelay = 0.1f;
    public float hitboxOffset = 1.25f;

    // Variables needed for Shooting
    [SerializeField] private GameObject projectileObject;
    public float shootDelay = 0.1f;
    public float projectileSpeed = 1;
    private int damageIncrease = 0;
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
        if (Input.GetButtonDown("Attack") && canAttack)
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

        // Logic for increasing damage based on number of enemies nearby
        damageIncrease = 0;
        damageIncrease += enemyCheck.numberOfEnemies;

        // Logic for shooting Projectile
        if (Input.GetButton("Shoot") && canAttack)
        {
            canAttack = false;

            GameObject projectile = Instantiate(projectileObject, transform.position, Quaternion.identity);

            // Correctly get direction projectile will travel at
            Vector2 direction = (playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f)) - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction;
            projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<Rigidbody2D>().velocity.normalized * projectileSpeed;

            projectile.GetComponent<ProjectileAttackScript>().damage += damageIncrease;

            StartCoroutine(EnableProjectile());
        }

        // Logic for increasing the super meter by a certain rate
        if (currentEnergy < 100)
            currentEnergy += (energyRate * (enemyCheck.numberOfEnemies + 1)) * Time.deltaTime;
        if (currentEnergy > 100)
            currentEnergy = 100;
        if (currentEnergy == 100)
            canUnleashUltimateDestruction = true;
        superMeter.SetBarValue(Mathf.Round(currentEnergy));

        // Logic for performing a super attack
        if (Input.GetButtonDown("Super") && canAttack && canUnleashUltimateDestruction)
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
        yield return new WaitForSeconds(attackDelay);
        attackHitbox.SetActive(false);
        canAttack = true;
    }

    IEnumerator EnableProjectile()
    {
        yield return new WaitForSeconds(shootDelay);
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
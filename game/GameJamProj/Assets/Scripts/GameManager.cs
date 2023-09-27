/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles the state of the game (waves, player status, score)
**/

using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Access to player's scripts
    private GameObject entityPlayer;
    private PlayerController playerController;
    private PlayerAttack playerAttack;
    private EnemyProximityCheck enemyProximityCheck;

    // Variables regarding score and wave number
    public int wave = 1;
    public float waveDuration = 60.0f;
    public float score = 0;
    public float scoreRate = 10.0f;

    // Other variables needed
    [SerializeField] private EnemySpawner[] enemySpawners;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GenericBar waveTimer;
    private float timer = 0.0f;
    public bool ended = false;

    void Start()
    {
        entityPlayer = GameObject.Find("EntityPlayer");
        playerController = entityPlayer.GetComponent<PlayerController>();
        playerAttack = entityPlayer.GetComponent<PlayerAttack>();
        enemyProximityCheck = entityPlayer.GetComponentInChildren<EnemyProximityCheck>();

        timer = waveDuration;
        waveText.text = "WAVE " + wave;
    }

    void Update()
    {
        // Game ends when player's health reaches zero, final score is shown
        if(playerController.currentHealth <= 0)
        {
            playerController.currentHealth = 0;
            playerController.enabled = false;
            playerAttack.enabled = false;
            enemyProximityCheck.enabled = false;

            ended = true;
            Debug.Log("Player has died... " + "FINAL SCORE: " + score);
        }

        // If player is still alive, continue adding to score
        if(!ended)
        {
            score += (scoreRate * (enemyProximityCheck.numberOfEnemies + 1)) * Time.deltaTime;

            if (timer <= 0.0f)
            {
                timer = 0.0f;
                wave++;
                StartCoroutine(PrepareNextWave());
                waveText.text = "WAVE " + wave;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            waveTimer.SetBarValue(timer, waveDuration);
        }
    }

    // Prepare next wave by pausing enemy spawners and giving player some health
    // If a certain number of waves pass, increase spawn rate of enemies
    IEnumerator PrepareNextWave()
    {
        ended = true;

        if (wave % 5 == 0)
        {
            foreach (EnemySpawner enemySpawner in enemySpawners)
            {
                enemySpawner.spawnRate++;
            }
            waveDuration += 5.0f;
        }
        timer = waveDuration;

        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.CancelInvoke("SpawnEnemies");
            enemySpawner.InvokeRepeating("SpawnEnemies", enemySpawner.startTime, enemySpawner.spawnInterval);
        }

        playerController.currentHealth += 50;
        if (playerController.currentHealth > playerController.maxHealth)
            playerController.currentHealth = playerController.maxHealth;

        yield return new WaitForSeconds(enemySpawners[0].startTime);

        ended = false;
    }
}
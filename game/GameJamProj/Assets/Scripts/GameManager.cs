/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script handles the state of the game (waves, player status, score)
**/

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Access to player's scripts
    private GameObject entityPlayer;
    private PlayerController playerController;
    private PlayerAttack playerAttack;
    private EnemyProximityCheck enemyProximityCheck;
    private RangedWeaponManager playerWeapons;

    // Variables regarding score and wave number
    public int wave = 1;
    public float waveDuration = 60.0f;
    public float score = 0;
    public float scoreRate = 10.0f;

    private readonly int waveDifficultyIncreaseInterval = 3; // For every # waves, difficulty increases
    private readonly int wavePlayerHealthRecovery = 50; // How much health player recovers at the start of new wave
    private readonly float waveDurationTimeIncrease = 5.0f; // How much longer a wave lasts when difficulty increases

    // Other variables needed
    [SerializeField] private EnemySpawner[] enemySpawners;
    [SerializeField] private PowerUpSpawn powerupSpawner;
    [SerializeField] private GameObject miniBoss;
    [SerializeField] private AudioSource waveComplete;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreMultiplier;
    [SerializeField] private GenericBar waveTimer;
    private float timer = 0.0f;
    private bool inGame = true;
    private bool isDead = false;
    public bool bossAppeared = false;
    public bool hasEnded = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        entityPlayer = GameObject.Find("EntityPlayer");
        playerController = entityPlayer.GetComponent<PlayerController>();
        playerAttack = entityPlayer.GetComponent<PlayerAttack>();
        enemyProximityCheck = entityPlayer.GetComponentInChildren<EnemyProximityCheck>();
        playerWeapons = entityPlayer.GetComponent<RangedWeaponManager>();

        timer = waveDuration;
        waveText.text = "WAVE " + wave;
    }

    void Update()
    {
        // Game ends when player's health reaches zero, final score is shown
        if(playerController.currentHealth <= 0 && inGame)
        {
            playerController.currentHealth = 0;

            playerController.enabled = false;
            playerAttack.enabled = false;
            playerWeapons.enabled = false;
            enemyProximityCheck.enabled = false;

            isDead = true;
            inGame = false;

            SceneManager.LoadScene("GameOver");
            SceneManager.sceneLoaded += OnGameOver;
        }

        // If player is still alive, continue adding to score
        if(!isDead)
        {
            score += (scoreRate * (enemyProximityCheck.numberOfEnemies + wave)) * Time.deltaTime;
            int roundedScore = Mathf.RoundToInt(score);
            scoreText.text = roundedScore.ToString("D9");
            scoreMultiplier.text = (enemyProximityCheck.numberOfEnemies + wave).ToString("D2");

            if (timer <= 0.0f) // After a certain amount of time passes, spawn miniboss
            {
                timer = 0.0f;
                if(!bossAppeared)
                    SpawnMiniBoss();
            }
            else
            {
                timer -= Time.deltaTime;
            }

            waveTimer.SetBarValue(timer, waveDuration);
        }
    }

    private void SpawnMiniBoss()
    {
        EnemySpawner randomSpawner = enemySpawners[Random.Range(0, enemySpawners.Length)];
        float xPosition = randomSpawner.xTransformPosition + Random.Range(-randomSpawner.xRange, randomSpawner.xRange);
        float yPosition = randomSpawner.yTransformPosition + Random.Range(-randomSpawner.yRange, randomSpawner.yRange);
        Vector2 spawnPosition = new(xPosition, yPosition);

        Instantiate(miniBoss, spawnPosition, Quaternion.identity);
        bossAppeared = true;
    }

    public void PrepareNextWave()
    {
        StartCoroutine(StartNextWave());
    }

    // Prepare next wave by pausing enemy spawners and giving player some health
    // If a certain number of waves pass, increase spawn rate of enemies
    IEnumerator StartNextWave()
    {
        hasEnded = true;
        wave++;
        waveText.text = "WAVE " + wave;
        waveComplete.PlayOneShot(waveComplete.clip);

        if (wave % waveDifficultyIncreaseInterval == 0)
        {
            foreach (EnemySpawner enemySpawner in enemySpawners)
            {
                enemySpawner.spawnRate++;
            }
            waveDuration += waveDurationTimeIncrease;
        }
        timer = waveDuration;

        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.CancelInvoke("SpawnEnemies");
            enemySpawner.InvokeRepeating("SpawnEnemies", enemySpawner.startTime, enemySpawner.spawnInterval);
        }

        powerupSpawner.ClearPowerUps();
        powerupSpawner.SpawnPowerUps();

        playerController.currentHealth += wavePlayerHealthRecovery;
        if (playerController.currentHealth > playerController.maxHealth)
            playerController.currentHealth = playerController.maxHealth;

        playerWeapons.ResetWeapons();

        yield return new WaitForSeconds(enemySpawners[0].startTime);

        hasEnded = false;
    }

    private void OnGameOver(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameOver") // Display Final Score on Game Over screen
        {
            GameObject finalScore = GameObject.Find("MainMenu/ScoreGroup/Score");
            int roundedScore = Mathf.RoundToInt(score);
            finalScore.GetComponent<TextMeshProUGUI>().text = roundedScore.ToString("D9");
        }
        else
        {
            SceneManager.sceneLoaded -= OnGameOver;
            Destroy(gameObject);
        }
    }
}
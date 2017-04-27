// Author(s): Paul Calande
// Wave controller for spawning waves of enemies.
// The key points are the enemy spawn points.

// Comment out the following line to prevent the current wave from being printed to the console.
//#define PRINT_WAVE_COUNTER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Tooltip("Reference to the enemy controller component.")]
    public EnemyController enemyController;
    [Tooltip("The total number of enemies to spawn in the wave.")]
    public int enemiesPerWave = 15;
    [Tooltip("After each wave, the current enemiesPerWave is multiplied by this quantity.")]
    public float enemiesPerWaveMultiplier = 1.2f;
    [Tooltip("Number of seconds between each enemy spawn.")]
    public float timeBetweenEnemies = 5f;
    [Tooltip("After each wave, the current timeBetweenEnemies is multiplied by this quantity.")]
    public float timeBetweenEnemiesMultiplier = 0.9f;
    [Tooltip("Number of seconds between each wave.")]
    public float timeBetweenWaves = 5f;
    [Tooltip("How much health the enemy has.")]
    public int enemyHealth = 8;
    [Tooltip("How much the enemy health increases between waves.")]
    public int enemyHealthIncrease = 1;
    [Tooltip("How much the enemy speed is multiplied by.")]
    public float enemySpeedMultiplier = 1f;
    [Tooltip("How much the enemy speed is multiplied by per wave.")]
    public float enemySpeedMultiplierIncrease = 0.1f;

    public delegate void WaveStartedHandler(int number);
    public event WaveStartedHandler WaveStarted;

    // Timer variables.
    private float timerBetweenEnemies;
    private float timerBetweenWaves;
    // List of the transforms of the enemy spawn points.
    private List<Transform> spawnPoints;
    // The current wave.
    private int wave = 0;
    // The current spawn point.
    private Transform currentSpawnPoint;
    // How many enemies have been spawned so far in this wave.
    private int enemiesSpawnedThisWave = 0;

    // Component references.
    private KeyPoints kp;
    private TimeScale ts;

    private void Awake()
    {
        kp = GetComponent<KeyPoints>();
        ts = GetComponent<TimeScale>();
    }

    private void Start()
    {
        // Get the enemy spawn points.
        spawnPoints = kp.GetKeyPoints();
        // State before the first wave.
        OnWaveStarted(0);
        // Start the countdown for the first wave.
        StartNextWaveTimer();
    }

    private void Update()
    {
        float timePassed = ts.GetTimePassed();
        if (timerBetweenWaves > 0f)
        {
            timerBetweenWaves -= timePassed;
            if (timerBetweenWaves <= 0f)
            {
                StartWave();
            }
        }
        else
        {
            // Manages the enemy spawning loop during waves.
            if (enemiesSpawnedThisWave < enemiesPerWave)
            {
                timerBetweenEnemies -= timePassed;
                while (timerBetweenEnemies <= 0f)
                {
                    timerBetweenEnemies += timeBetweenEnemies;
                    SpawnEnemy();
                }
                // Once all of the enemies have been spawned, end the wave.
                if (enemiesSpawnedThisWave == enemiesPerWave)
                {
                    EndWave();
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        // Choose a spawn point at which to spawn an enemy.
        ChooseRandomSpawnPoint();
        // Spawn the actual enemy.
        enemyController.SpawnEnemy(currentSpawnPoint, enemyHealth, enemySpeedMultiplier);
        // Increment the number of enemies spawned this wave.
        enemiesSpawnedThisWave += 1;
    }

    // Returns the current wave ID.
    public int GetCurrentWave()
    {
        return wave;
    }

    // Choose a random spawn point.
    public void ChooseRandomSpawnPoint()
    {
        currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    // Start the timer for the next wave.
    private void StartNextWaveTimer()
    {
        timerBetweenWaves = timeBetweenWaves;
    }

    private void StartWave()
    {
        // Increment the wave counter.
        wave += 1;
#if PRINT_WAVE_COUNTER
        Debug.Log("Wave " + wave + " has begun!");
#endif
        // Invoke the wave started event.
        OnWaveStarted(wave);

        // Begin spawning the enemies.
        timerBetweenEnemies = 0f;
    }

    // This function is called when a wave ends.
    private void EndWave()
    {
        // Calculate deltas to affect the next wave's difficulty.
        enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * enemiesPerWaveMultiplier);
        timeBetweenEnemies *= timeBetweenEnemiesMultiplier;
        enemyHealth += enemyHealthIncrease;
        enemySpeedMultiplier += enemySpeedMultiplierIncrease;

        // Reset various counter variables.
        enemiesSpawnedThisWave = 0;

        // Prepare for the next wave.
        StartNextWaveTimer();
    }

    // Event invocations.
    private void OnWaveStarted(int number)
    {
        if (WaveStarted != null)
        {
            WaveStarted(number);
        }
    }
}
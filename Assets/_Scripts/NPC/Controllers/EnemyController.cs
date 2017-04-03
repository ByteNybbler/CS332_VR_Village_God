// Author(s): Paul Calande
// Enemy controller class.
// The key points are the enemy spawn points.

// Comment out the following line to prevent the current wave from being printed to the console.
#define PRINT_WAVE_COUNTER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (KeyPoints))]
public class EnemyController : LateInit
{
    [Tooltip("Prefab of the enemies to be spawned.")]
    public GameObject enemyPrefab;
    [Tooltip("Reference to the village GameObject.")]
    public GameObject villageInstance;
    [Tooltip("The total number of enemies to spawn in the wave.")]
    public int enemiesPerWave = 15;
    [Tooltip("After each wave, the current enemiesPerWave is multiplied by this quantity.")]
    public float enemiesPerWaveMultiplier = 1.2f;
    [Tooltip("Number of seconds between each enemy spawn.")]
    public float timeBetweenEnemies = 1f;
    [Tooltip("After each wave, the current timeBetweenEnemies is multiplied by this quantity.")]
    public float timeBetweenEnemiesMultiplier = 0.9f;
    [Tooltip("Number of seconds between each wave.")]
    public float timeBetweenWaves;

    public delegate void EnemyDiedHandler(GameObject enemy, int xp);
    public event EnemyDiedHandler EnemyDied;

    // List of the transforms of the enemy spawn points.
    private List<Transform> spawnPoints;
    // The current wave.
    private int wave = 0;
    // The current spawn point.
    private Transform currentSpawnPoint;
    // How many enemies have been spawned so far in this wave.
    private int enemiesSpawnedThisWave = 0;
    // List of living enemies.
    private List<GameObject> enemies = new List<GameObject>();

    // Component references.
    private KeyPoints kp;
    private Village villageComponent;

    private void Awake()
    {
        kp = GetComponent<KeyPoints>();
    }

    public override void Init()
    {
        villageComponent = villageInstance.GetComponent<Village>();
        // Get the enemy spawn points.
        spawnPoints = kp.GetKeyPoints();
        // Start the countdown for the first wave.
        StartNextWaveTimer();
        base.Init();
    }

    // Choose a random spawn point.
    public void ChooseRandomSpawnPoint()
    {
        currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    // Spawn an enemy.
    public void SpawnEnemy()
    {
        // Choose a spawn point at which to spawn an enemy.
        ChooseRandomSpawnPoint();

        // Instantiate an enemy.
        GameObject enemy = Instantiate(enemyPrefab, currentSpawnPoint.position, currentSpawnPoint.rotation);
        // Pass the village component to the spawned enemy.
        EnemyStatus es = enemy.GetComponent<EnemyStatus>();
        es.village = villageComponent;
        es.Died += EnemyStatus_Died;
        // Increment the number of enemies spawned this wave.
        enemiesSpawnedThisWave += 1;
        // Add the enemy to the list.
        enemies.Add(enemy);
        // Initialize the enemy.
        es.Init();
    }

    private void EnemyStatus_Died(GameObject enemy, int xp)
    {
        OnEnemyDied(enemy, xp);
    }

    private void OnEnemyDied(GameObject enemy, int xp)
    {
        if (EnemyDied != null)
        {
            EnemyDied(enemy, xp);
        }
    }

    protected override void EventsSubscribe()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyStatus>().Died += EnemyStatus_Died;
            }
        }
    }
    protected override void EventsUnsubscribe()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyStatus>().Died -= EnemyStatus_Died;
            }
        }
    }

    // Returns the current wave ID.
    public int GetCurrentWave()
    {
        return wave;
    }

    // Start the timer for the next wave.
    private void StartNextWaveTimer()
    {
        StartCoroutine(NextWaveTimer());
    }

    private void StartWave()
    {
        // Increment the wave counter.
        wave += 1;
#if PRINT_WAVE_COUNTER
        Debug.Log("Wave " + wave + " has begun!");
#endif
        // Start the coroutine to begin spawning the enemies.
        StartCoroutine(WaveCoroutine());
    }

    // This function is called when a wave ends.
    private void EndWave()
    {
        // Calculate deltas to affect the next wave's difficulty.
        enemiesPerWave = (int)(enemiesPerWave*enemiesPerWaveMultiplier);
        timeBetweenEnemies *= timeBetweenEnemiesMultiplier;

        // Reset various counter variables.
        enemiesSpawnedThisWave = 0;

        // Prepare for the next wave.
        StartNextWaveTimer();
    }

    // This coroutine manages the cooldown time between waves.
    IEnumerator NextWaveTimer()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartWave();
    }

    // This coroutine manages the enemy spawning loop during waves.
    IEnumerator WaveCoroutine()
    {
        while (enemiesSpawnedThisWave < enemiesPerWave)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
        // Once all of the enemies have been spawned, end the wave.
        EndWave();
    }
}
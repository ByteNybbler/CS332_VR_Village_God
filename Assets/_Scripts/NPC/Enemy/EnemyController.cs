// Author(s): Paul Calande
// Enemy controller class.
// The key points are the enemy spawn points.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (KeyPoints))]
public class EnemyController : MonoBehaviour
{
    [Tooltip("Prefab of the enemies to be spawned.")]
    public GameObject enemyPrefab;

    // List of the transforms of the enemy spawn points.
    private List<Transform> spawnPoints;
    // The current wave.
    private int wave = 1;
    // The current spawn point.
    private Transform currentSpawnPoint;
    // Component references.
    private KeyPoints kp;
    
    private void Start()
    {
        kp = GetComponent<KeyPoints>();
        spawnPoints = kp.GetKeyPoints();
    }

    public void ChooseRandomSpawnPoint()
    {
        currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    public void SpawnEnemy()
    {
        ChooseRandomSpawnPoint();
        Instantiate(enemyPrefab, currentSpawnPoint.position, currentSpawnPoint.rotation);
    }

    public int GetCurrentWave()
    {
        return wave;
    }
}
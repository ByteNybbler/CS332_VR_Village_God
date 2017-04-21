// Author(s): Paul Calande
// Enemy controller class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (KeyPoints))]
public class EnemyController : MonoBehaviour
{
    [Tooltip("Prefab of the enemies to be spawned.")]
    public GameObject enemyPrefab;
    [Tooltip("Reference to the village class instance.")]
    public Village village;

    public delegate void EnemyDiedHandler(GameObject enemy, int xp);
    public event EnemyDiedHandler EnemyDied;

    // List of living enemies.
    private List<GameObject> enemies = new List<GameObject>();

    // Spawn an enemy at a transform.
    public GameObject SpawnEnemy(Transform trans)
    {
        // Instantiate an enemy.
        GameObject enemy = Instantiate(enemyPrefab, trans.position, trans.rotation);
        // Pass the village component to the spawned enemy.
        EnemyStatus es = enemy.GetComponent<EnemyStatus>();
        es.village = village;
        es.Died += EnemyStatus_Died;
        // Initialize the enemy.
        es.Start();
        // Add the enemy to the list.
        enemies.Add(enemy);
        // Return the enemy!
        return enemy;
    }

    private void OnDestroy()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyStatus>().Died -= EnemyStatus_Died;
            }
        }
    }

    private void EnemyStatus_Died(GameObject enemy, int xp)
    {
        // Remove the enemy from the enemies list.
        enemies.Remove(enemy);
        // Invoke the "enemy died" event.
        OnEnemyDied(enemy, xp);
    }

    private void OnEnemyDied(GameObject enemy, int xp)
    {
        if (EnemyDied != null)
        {
            EnemyDied(enemy, xp);
        }
    }
}
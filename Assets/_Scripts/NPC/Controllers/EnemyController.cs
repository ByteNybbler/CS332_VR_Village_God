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
    public GameObject SpawnEnemy(Transform trans, int health, float speedMultiplier)
    {
        // Instantiate an enemy.
        GameObject enemy = Instantiate(enemyPrefab, trans.position, trans.rotation);
        // Get the enemy's components.
        EnemyStatus es = enemy.GetComponent<EnemyStatus>();
        EnemyMovement em = enemy.GetComponent<EnemyMovement>();
        Health h = enemy.GetComponent<Health>();
        TimeControllable tc = enemy.GetComponent<TimeControllable>();
        // Pass variables to the spawned enemy.
        es.village = village;
        es.Died += EnemyStatus_Died;
        tc.timeController = GetComponent<TimeControllable>().timeController;
        TimeScale.PassTimeScale(enemy, gameObject);
        // Adjust the enemy strength based on the parameters.
        h.SetMaxHealth(health, Health.Type.Null);
        h.SetHealth(health, Health.Type.Null);
        em.MultiplySpeed(speedMultiplier);
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

    public void KillAllEnemies()
    {
        while (enemies.Count != 0)
        {
            enemies[0].GetComponent<NPCHealth>().Die();
        }
        /*
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<NPCHealth>().Die();
        }
        */
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
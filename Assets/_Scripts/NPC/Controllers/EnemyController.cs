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

    public delegate void EnemyDiedHandler(EnemyStatus enemy, int xp);
    public event EnemyDiedHandler EnemyDied;

    // List of living enemies.
    private List<EnemyStatus> enemies = new List<EnemyStatus>();

    // Spawn an enemy at a transform.
    public GameObject SpawnEnemy(Transform trans, int health, float speedMultiplier, int faith)
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
        es.faithOnKill = faith;
        tc.timeController = GetComponent<TimeControllable>().timeController;
        TimeScale.PassTimeScale(enemy, gameObject);
        // Adjust the enemy strength based on the parameters.
        h.SetMaxHealth(health, Health.Type.Null);
        h.SetHealth(health, Health.Type.Null);
        em.MultiplySpeed(speedMultiplier);
        // Add the enemy to the list.
        enemies.Add(es);
        // Return the enemy!
        return enemy;
    }

    private void OnDestroy()
    {
        foreach (EnemyStatus enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Died -= EnemyStatus_Died;
            }
        }
    }

    public void KillAllEnemies()
    {
        while (enemies.Count != 0)
        {
            enemies[0].Die();
        }
    }

    private void EnemyStatus_Died(EnemyStatus enemy, int faith)
    {
        // Remove the enemy from the enemies list.
        enemies.Remove(enemy);
        // Invoke the "enemy died" event.
        OnEnemyDied(enemy, faith);
    }

    private void OnEnemyDied(EnemyStatus enemy, int faith)
    {
        if (EnemyDied != null)
        {
            EnemyDied(enemy, faith);
        }
    }
}
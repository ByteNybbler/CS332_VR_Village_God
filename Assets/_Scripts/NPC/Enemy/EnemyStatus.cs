// Author(s): Paul Calande
// Enemy component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyStatus : LateInit
{
    [Tooltip("Reference to the village component so the enemy can keep track of villagers.")]
    public Village village;
    [Tooltip("How much XP is rewarded when the enemy is killed.")]
    public int xpOnKill = 5;

    public delegate void DiedHandler(GameObject victim, int xp);
    public event DiedHandler Died;

    // Component references.
    private Health health;
    private EnemyMovement cmpEnemyMovement;

    private void Awake()
    {
        health = GetComponent<Health>();
        cmpEnemyMovement = GetComponent<EnemyMovement>();
    }

    public override void Init()
    {
        SetRandomTarget();
        base.Init();
    }

    protected override void EventsSubscribe()
    {
        health.Died += Health_Died;
        village.VillagerDied += Village_VillagerDied;
    }
    protected override void EventsUnsubscribe()
    {
        health.Died -= Health_Died;
        village.VillagerDied -= Village_VillagerDied;
    }

    private void OnDied(GameObject obj, int xp)
    {
        if (Died != null)
        {
            Died(obj, xp);
        }
    }

    // Health death event payload.
    private void Health_Died()
    {
        // Invoke this enemy's death event.
        OnDied(gameObject, xpOnKill);
        // Destroy the enemy.
        Destroy(gameObject);
    }

    // Event payload for when the target villager dies.
    private void Village_VillagerDied(GameObject victim)
    {
        // If the current target was removed from the villager list...
        if (victim == cmpEnemyMovement.target)
        {
            // Choose a new target.
            SetRandomTarget();
        }
    }

    // Choose a certain villager to target.
    public void SetTarget(GameObject newTarget)
    {
        cmpEnemyMovement.target = newTarget;
    }

    // Choose a random villager to target.
    public void SetRandomTarget()
    {
        SetTarget(village.GetRandomVillager());
    }
}
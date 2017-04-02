// Author(s): Paul Calande
// Enemy component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyStatus : MonoBehaviour
{
    [Tooltip("Reference to the village component so the enemy can keep track of villagers.")]
    public Village village;
    [Tooltip("How much XP is rewarded when the enemy is killed.")]
    public int xpOnKill = 5;

    public delegate void DieAction(GameObject victim, int xp);
    public event DieAction OnDeath;

    // Component references.
    private Health health;
    private EnemyMovement cmpEnemyMovement;

    private void Awake()
    {
        health = GetComponent<Health>();
        cmpEnemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        SetRandomTarget();
    }

    private void OnEnable()
    {
        health.OnDeath += Die;

        village.OnVillagerDie += VillagerRemovedFromList;
        //cmpEnemyMovement.target.GetComponent<VillagerStatus>().OnDeath -= TargetDie;
    }
    private void OnDisable()
    {
        health.OnDeath -= Die;

        village.OnVillagerDie -= VillagerRemovedFromList;
        //cmpEnemyMovement.target.GetComponent<VillagerStatus>().OnDeath -= TargetDie;
    }

    // Health death event payload.
    private void Die()
    {
        // Invoke this enemy's death event.
        OnDeath(gameObject, xpOnKill);
        // Destroy the enemy.
        Destroy(gameObject);
    }

    // Event payload for when the target villager dies.
    private void VillagerRemovedFromList(GameObject victim)
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
        /*
        if (cmpEnemyMovement.target != null)
        {
            cmpEnemyMovement.target.GetComponent<VillagerStatus>().OnDeath -= TargetDie;
        }
        */
        cmpEnemyMovement.target = newTarget;
        //newTarget.GetComponent<VillagerStatus>().OnDeath += TargetDie;
    }

    // Choose a random villager to target.
    public void SetRandomTarget()
    {
        SetTarget(village.GetRandomVillager());
    }
}
// Author(s): Paul Calande
// Enemy component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(NPCHealth))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyStatus : MonoBehaviour
{
    [Tooltip("Reference to the village class instance.")]
    public Village village;
    [Tooltip("How much XP is rewarded when the enemy is killed.")]
    public int xpOnKill = 5;

    public delegate void DiedHandler(EnemyStatus victim, int xp);
    public event DiedHandler Died;

    // Component references.
    private NPCHealth npchealth;
    private EnemyMovement cmpEnemyMovement;

    private void Awake()
    {
        npchealth = GetComponent<NPCHealth>();
        npchealth.Died += NPCHealth_Died;
        cmpEnemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        village.VillagerDied += Village_VillagerDied;
        SetRandomTarget();
    }

    private void OnDestroy()
    {
        if (npchealth != null)
        {
            npchealth.Died -= NPCHealth_Died;
        }
        if (village != null)
        {
            village.VillagerDied -= Village_VillagerDied;
        }
    }

    // Choose a certain villager to target.
    public void SetTarget(VillagerStatus newTarget)
    {
        cmpEnemyMovement.target = newTarget;
    }

    // Choose a random villager to target.
    public void SetRandomTarget()
    {
        SetTarget(village.GetRandomVillager());
    }

    public void Die()
    {
        npchealth.Die();
    }

    // NPC health death event payload.
    private void NPCHealth_Died()
    {
        // Invoke this enemy's death event.
        OnDied(this, xpOnKill);
    }

    // Event payload for when a villager dies.
    private void Village_VillagerDied(VillagerStatus victim)
    {
        // If the current target was removed from the villager list...
        if (victim == cmpEnemyMovement.target)
        {
            // Choose a new target.
            SetRandomTarget();
        }
    }

    // Event invocations.
    private void OnDied(EnemyStatus obj, int xp)
    {
        if (Died != null)
        {
            Died(obj, xp);
        }
    }
}
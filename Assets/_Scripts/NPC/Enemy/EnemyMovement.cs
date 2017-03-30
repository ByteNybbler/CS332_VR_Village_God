// Author(s): Paul Calande
// Enemy AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [Tooltip("Reference to the village component so the enemy can keep track of villagers.")]
    public Village village;
    [Tooltip("How much XP is rewarded when the enemy is killed.")]
    public int xpOnKill = 5;

    // The current villager being targeted.
    private GameObject target = null;
    // Component references.
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        Health.OnDeath += SomeoneDied;
    }
    private void OnDisable()
    {
        Health.OnDeath -= SomeoneDied;
    }

    private void Update()
    {
        // If the target exists...
        if (target != null)
        {
            // Move towards the target.
            agent.destination = target.transform.position;
        }
    }

    private void SomeoneDied(GameObject victim)
    {
        // If the enemy is the victim...
        if (victim == gameObject)
        {
            // Destroy it.
            Destroy(gameObject);
            // Increment XP.
            Levels.AddXP(xpOnKill);
        }
        // If the current target dies...
        if (victim == target)
        {
            // Choose a new target.
            SetRandomTarget();
        }
    }

    // Choose a certain villager to target.
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    // Choose a random villager to target.
    public void SetRandomTarget()
    {
        SetTarget(village.GetRandomVillager());
    }
}
﻿// Author(s): Paul Calande
// Enemy AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [Tooltip("The current villager being targeted.")]
    public GameObject target = null;
    [Tooltip("How close to the villager the enemy has to be to deal damage.")]
    public float damageDistance;
    [Tooltip("How many seconds between each attempt to damage the villager.")]
    public float timeBetweenAttacks;

    // Timers.
    private float timerBetweenAttacks;

    // Component references.
    private NavMeshAgent agent;
    private TimeScale ts;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ts = GetComponent<TimeScale>();
    }

    private void Start()
    {
        timerBetweenAttacks = timeBetweenAttacks;
    }

    private void Update()
    {
        // If the target exists...
        if (target != null)
        {
            // Move towards the target.
            agent.destination = target.transform.position;
            // Occasionally attempt to deal damage towards the target.
            float timePassed = ts.GetTimePassed();
            timerBetweenAttacks -= timePassed;
            while (timerBetweenAttacks <= 0f)
            {
                timerBetweenAttacks += timeBetweenAttacks;

                float distance = Vector3.Distance(transform.position, target.transform.position);
                // If the enemy is close enough to the target to deal damage...
                if (distance < damageDistance)
                {
                    // Damage the target.
                    Health compVillagerHealth = target.GetComponent<Health>();
                    int damage = 1;
                    compVillagerHealth.Damage(damage);
                }
            }
        }
    }
}
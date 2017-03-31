// Author(s): Paul Calande
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

    // Component references.
    private NavMeshAgent agent;
    private EnemyStatus enemyStatusComponent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Move towards the target.
        agent.destination = target.transform.position;
    }
}
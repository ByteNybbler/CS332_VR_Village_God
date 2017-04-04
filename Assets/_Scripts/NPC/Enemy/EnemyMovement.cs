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
    [Tooltip("How close to the villager the enemy has to be to deal damage.")]
    public float damageDistance;
    [Tooltip("How many seconds between each attempt to damage the villager.")]
    public float timeBetweenAttacks;

    // Component references.
    private NavMeshAgent agent;
    // RisingTextCreator for damage text.
    private RisingTextCreator rtcDamage;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rtcDamage = GetComponent<RisingTextCreator>();
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

    private void OnEnable()
    {
        StartCoroutine(TryToDealDamage());
    }

    IEnumerator TryToDealDamage()
    {
        while (true)
        {
            // If the target exists...
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                // If the enemy is close enough to the target to deal damage...
                if (distance < damageDistance)
                {
                    Health compVillagerHealth = target.GetComponent<Health>();
                    float damage = 1f;
                    // Create the damage text.
                    rtcDamage.message = "-" + (int)damage + " HP";
                    rtcDamage.CreateRisingText(target.transform.position);
                    // Damage the target.
                    compVillagerHealth.Damage(damage);
                }
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }
}
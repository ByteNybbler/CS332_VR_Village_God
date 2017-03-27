﻿// Author(s): Paul Calande
// Villager interaction script for VR functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRTK;

public class VillagerInteract : VRTK_InteractableObject
{
    [Tooltip("How many seconds to wait between each attempt to snap to the navmesh.")]
    public float enableAgentFrequency;
    [Tooltip("How close the agent must be to the navmesh before being snapped to it and enabled again.")]
    public float enableAgentDistance;

    // Component references.
    private NavMeshAgent agent;
    private VillagerMovement vm;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        vm = GetComponent<VillagerMovement>();
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        SetBehavior(false);
        StopAllCoroutines();
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        StartCoroutine(TryToEnableAgent());
    }

    private void SetBehavior(bool isEnabled)
    {
        agent.enabled = isEnabled;
        vm.enabled = isEnabled;
    }

    // Coroutine to try to turn the agent and villager movement components back on.
    // The components will only be enabled if...
    // The villager is a certain distance from the navmesh.
    IEnumerator TryToEnableAgent()
    {
        while (true)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, enableAgentDistance, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                SetBehavior(true);
                break;
            }
            else
            {
                yield return new WaitForSeconds(enableAgentFrequency);
            }
        }
    }
}
// Author(s): Paul Calande
// Villager interaction script for VR functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRTK;

public class VillagerInteract : VRTK_InteractableObject
{
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
        agent.enabled = false;
        vm.enabled = false;
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        agent.enabled = true;
        vm.enabled = true;
    }
}
// Author(s): Paul Calande
// Villager interaction script for VR functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRTK;

public class NPCInteract : VRTK_InteractableObject
{
    [Tooltip("How many seconds to wait between each attempt to snap to the navmesh.")]
    public float enableAgentFrequency;
    [Tooltip("How close the agent must be to the navmesh before being snapped to it and enabled again.")]
    public float enableAgentDistance;
    [Tooltip("Reference to the NPC's movement component.\n" + 
        "This component is enabled and disabled based on whether the NPC is currently being grabbed.")]
    public MonoBehaviour movementComponent;

    // Component references.
    private NavMeshAgent agent;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        SetKinematicEnabled(true);
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        SetAgentEnabled(false);
        SetKinematicEnabled(true);
        StopAllCoroutines();
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        SetKinematicEnabled(false);
        StartCoroutine(TryToEnableAgent());
    }

    /// <summary>
    /// Set the agent and movement behavior state of the NPC.
    /// </summary>
    /// <param name="isEnabled">True for agent and movement to be enabled, false to be disabled.</param>
    private void SetAgentEnabled(bool isEnabled)
    {
        agent.enabled = isEnabled;
        movementComponent.enabled = isEnabled;
    }

    /// <summary>
    /// Set the rigidbody's kinematic state.
    /// </summary>
    /// <param name="isEnabled"></param>
    private void SetKinematicEnabled(bool isEnabled)
    {
        rb.isKinematic = isEnabled;
        //rb.detectCollisions = !isEnabled;
    }

    // Coroutine to try to turn the agent and NPC movement components back on.
    // The components will only be enabled if the agent is a certain distance from the navmesh.
    IEnumerator TryToEnableAgent()
    {
        while (true)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, enableAgentDistance, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                SetAgentEnabled(true);
                SetKinematicEnabled(true);
                break;
            }
            else
            {
                yield return new WaitForSeconds(enableAgentFrequency);
            }
        }
    }
}
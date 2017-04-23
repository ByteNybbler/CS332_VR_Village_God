// Author(s): Paul Calande
// Villager interaction script for VR functionality.

// Comment out the following line to disable all rigidbody functionality.
#define NPC_USING_RIGIDBODY

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
#if NPC_USING_RIGIDBODY
    //private Rigidbody rb;
#endif

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
#if NPC_USING_RIGIDBODY
        //rb = GetComponent<Rigidbody>();
        //rb = interactableRigidbody;
        SetKinematicEnabled(true);
#endif
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        SetAgentEnabled(false);
#if NPC_USING_RIGIDBODY
        SetKinematicEnabled(false);
#endif
        StopAllCoroutines();
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
#if NPC_USING_RIGIDBODY
        //SetKinematicEnabled(false);
#endif
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

#if NPC_USING_RIGIDBODY
    /// <summary>
    /// Set the rigidbody's kinematic state.
    /// </summary>
    /// <param name="isEnabled">True to enable the rigidbody's kinematic flag.</param>
    private void SetKinematicEnabled(bool isEnabled)
    {
        //Debug.Log(interactableRigidbody);
        //rb.isKinematic = isEnabled;
        //rb.detectCollisions = !isEnabled;
        isKinematic = isEnabled;
    }
#endif

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
#if NPC_USING_RIGIDBODY
                SetKinematicEnabled(true);
#endif
                break;
            }
            else
            {
                yield return new WaitForSeconds(enableAgentFrequency);
            }
        }
    }
}
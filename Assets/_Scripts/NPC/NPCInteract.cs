// Author(s): Paul Calande
// Villager interaction script for VR functionality.

// Comment out the following line to disable all rigidbody functionality.
#define NPC_USING_RIGIDBODY
// Comment out the following line to disable Awake debug messages.
//#define NPC_AWAKE_DEBUG
// Comment out the following line to disable grab/ungrab debug messages.
//#define NPC_GRAB_DEBUG
// Comment out the following line to disable kinematic debug messages.
//#define NPC_KINEMATIC_DEBUG
// Comment out the following line to prevent debug messages involving the rigidbody's existence.
//#define NPC_RIGIDBODY_EXISTENCE_DEBUG

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
    private Rigidbody rb;
#endif

    protected override void Awake()
    {
		base.Awake ();
		agent = GetComponent<NavMeshAgent>();
#if NPC_AWAKE_DEBUG
		Debug.Log (name + " NPCInteract Awake()");
#if NPC_RIGIDBODY_EXISTENCE_DEBUG
		RigidbodyExistenceDebug();
#endif
#endif
#if NPC_USING_RIGIDBODY
		//rb = GetComponent<Rigidbody>();
        //rb = interactableRigidbody;
        SetKinematicEnabled(true);
#endif
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
#if NPC_GRAB_DEBUG
		Debug.Log ("Grabbed NPC.");
#endif
        SetAgentEnabled(false);
#if NPC_USING_RIGIDBODY
        SetKinematicEnabled(false);
#if NPC_RIGIDBODY_EXISTENCE_DEBUG
		RigidbodyExistenceDebug();
#endif
#if NPC_KINEMATIC_DEBUG
		Debug.Log(name + " isKinematic: " + isKinematic);
#endif
#endif
		base.Grabbed(currentGrabbingObject);
#if NPC_GRAB_DEBUG
		Debug.Log ("base.Grabbed NPC.");
#endif
		StopAllCoroutines();
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
#if NPC_GRAB_DEBUG
		Debug.Log ("Ungrabbed NPC.");
#endif
#if NPC_RIGIDBODY_EXISTENCE_DEBUG
		RigidbodyExistenceDebug();
#endif
#if NPC_KINEMATIC_DEBUG
		Debug.Log(name + " isKinematic (BEFORE BASE.UNGRABBED): " + isKinematic);
#endif
		base.Ungrabbed(previousGrabbingObject);
#if NPC_GRAB_DEBUG
		Debug.Log ("base.Ungrabbed NPC.");
#endif
#if NPC_USING_RIGIDBODY
        //SetKinematicEnabled(false);
#if NPC_RIGIDBODY_EXISTENCE_DEBUG
		RigidbodyExistenceDebug();
#endif
#if NPC_KINEMATIC_DEBUG
		Debug.Log(name + " isKinematic: " + isKinematic);
#endif
#endif
        StartCoroutine(TryToEnableAgent());
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
				//Debug.Log ("TryToEnableAgent() found success!");
                transform.position = hit.position;
                SetAgentEnabled(true);
				//ZeroVelocity ();
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
		//Debug.Log (name + " SetKinematicEnabled(" + isEnabled + ")");
		//Debug.Log(interactableRigidbody);
		//rb.isKinematic = isEnabled;
		//rb.detectCollisions = !isEnabled;
		isKinematic = isEnabled;
		SaveCurrentState ();
	}
#endif

#if NPC_RIGIDBODY_EXISTENCE_DEBUG
	private void RigidbodyExistenceDebug()
	{
		if (interactableRigidbody == null) {
			Debug.Log (name + " NPCInteract Rigidbody status: null...");
		} else {
			Debug.Log (name + " NPCInteract Rigidbody status: NOT NULL!");
		}
	}
#endif

	/*
	protected override void LoadPreviousState()
	{
		// Overriding this function because it caused problems with the Rigidbody functionality.
	}
	*/
}
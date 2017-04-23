// Author(s): Paul Calande
// NPC class for handling time stop interactions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCTime : MonoBehaviour
{
    // Original agent values.
    float oldSpeed, oldAngularSpeed, oldAcceleration;
    Vector3 oldVelocity;

    // Component references.
    private TimeScale ts;
    private NavMeshAgent agent;

    private void Awake()
    {
        ts = GetComponent<TimeScale>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ts.Changed += TimeScale_Changed;
        ts.Reset += TimeScale_Reset;

        oldSpeed = agent.speed;
        oldAngularSpeed = agent.angularSpeed;
        oldAcceleration = agent.acceleration;
    }

    private void OnDestroy()
    {
        if (ts != null)
        {
            ts.Changed -= TimeScale_Changed;
            ts.Reset -= TimeScale_Reset;
        }
    }

    private void TimeScale_Changed(float timescale)
    {
        //Debug.Log(name + " got caught in stopped time.");

        agent.speed = oldSpeed * timescale;
        agent.angularSpeed = oldAngularSpeed * timescale;
        agent.acceleration = oldAcceleration * timescale;

        oldVelocity = agent.velocity;
        agent.velocity *= timescale;
    }

    private void TimeScale_Reset()
    {
        agent.speed = oldSpeed;
        agent.angularSpeed = oldAngularSpeed;
        agent.acceleration = oldAcceleration;

        agent.velocity = oldVelocity;
    }
}
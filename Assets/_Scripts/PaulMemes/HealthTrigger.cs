﻿// Author(s): Paul Calande
// Script to be attached to a trigger that modifies an object's health.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrigger : MonoBehaviour
{
    public enum Type
    {
        Damage, Heal, SetHealth
    }

    [Tooltip("The trigger type.")]
    public Type triggerType;
    [Tooltip("The health type.")]
    public Health.Type healthType;
    [Tooltip("The quantity of health points that this trigger will affect the health with.")]
    public int amount = 1;
    [Tooltip("Seconds between each attempt to influence the object inside the trigger.")]
    public float timeBetweenFires = 0.5f;
    [Tooltip("The collider that's connected to this HealthTrigger.")]
    public Collider compCollider;

    public delegate void DisabledHandler(Collider col);
    public event DisabledHandler Disabled;

    private void OnDisable()
    {
        //Debug.Log("HealthTrigger OnDisable()");
        OnDisabled(compCollider);
    }

    private void OnDisabled(Collider col)
    {
        if (Disabled != null)
        {
            Disabled(col);
        }
    }
}
// Author(s): Paul Calande
// Script to be attached to a trigger that modifies an object's health.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrigger : MonoBehaviour
{
    public enum Type
    {
        damage, heal, setHealth
    }

    [Tooltip("The trigger type.")]
    public Type type;
    [Tooltip("The quantity of health points that this trigger will affect the health with.")]
    public float amount;
    [Tooltip("Seconds between each attempt to influence the object inside the trigger.")]
    public float timeBetweenFires;

    public delegate void DisabledHandler(Collider col);
    public event DisabledHandler Disabled;

    // Component references.
    private Collider compCollider;

    private void Awake()
    {
        compCollider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
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
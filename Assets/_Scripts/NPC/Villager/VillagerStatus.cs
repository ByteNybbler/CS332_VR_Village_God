// Author(s): Paul Calande
// Villager component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class VillagerStatus : MonoBehaviour
{
    public delegate void DieAction(GameObject victim);
    public event DieAction OnDeath;

    // Component references.
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += Die;
    }
    private void OnDisable()
    {
        health.OnDeath -= Die;
    }

    // Health death event payload.
    private void Die()
    {
        // Call the villager death event.
        if (OnDeath != null)
        {
            OnDeath(gameObject);
        }
        // Destroy this villager.
        Destroy(gameObject);
    }
}
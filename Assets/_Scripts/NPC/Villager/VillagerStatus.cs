// Author(s): Paul Calande
// Villager component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class VillagerStatus : MonoBehaviour
{
    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    // Component references.
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.Died += Health_Died;
    }
    private void OnDisable()
    {
        health.Died -= Health_Died;
    }

    // Health death event payload.
    private void Health_Died()
    {
        // Call the villager death event.
        OnDied(gameObject);
        // Destroy this villager.
        Destroy(gameObject);
    }

    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}
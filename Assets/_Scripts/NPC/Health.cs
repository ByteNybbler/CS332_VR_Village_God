// Author(s): Paul Calande
// Health script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("The maximum health the object can have.")]
    public int healthMax;
    [Tooltip("The current health the object has.")]
    public int healthCurrent;

    public delegate void DieAction();
    public event DieAction OnDeath;

    private void Awake()
    {
        healthCurrent = healthMax;
    }

    public void Damage(int amount)
    {
        healthCurrent -= amount;
        // Check if the HP has run out, and if so, DIE!!!
        if (healthCurrent <= 0)
        {
            if (OnDeath != null)
            {
                OnDeath();
            }
        }
    }

    public void Heal(int amount)
    {
        healthCurrent += amount;
        // Cap the HP.
        if (amount > healthMax)
        {
            amount = healthMax;
        }
    }
}
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

    public delegate void DiedHandler();
    public event DiedHandler Died;

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
            OnDied();
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

    private void OnDied()
    {
        if (Died != null)
        {
            Died();
        }
    }
}
// Author(s): Paul Calande
// Health script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("The maximum health the object can have.")]
    public float hpMax;

    public delegate void DieAction(GameObject victim);
    public static event DieAction OnDeath;

    // The current amount of HP the NPC has.
    private float hpCurrent;

    private void Awake()
    {
        hpCurrent = hpMax;
    }

    public void Damage(float amount)
    {
        hpCurrent -= amount;
        // Check if the HP has run out, and if so, DIE!!!
        if (hpCurrent <= 0)
        {
            if (OnDeath != null)
            {
                OnDeath(gameObject);
            }
        }
    }

    public void Heal(float amount)
    {
        hpCurrent += amount;
        // Cap the HP.
        if (amount > hpMax)
        {
            amount = hpMax;
        }
    }
}
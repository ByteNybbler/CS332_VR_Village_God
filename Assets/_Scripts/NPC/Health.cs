// Author(s): Paul Calande
// Health script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("The maximum health the object can have.")]
    public float healthMax;
    [Tooltip("The current health the object has.")]
    public float healthCurrent;

    public delegate void DiedHandler();
    public event DiedHandler Died;

    private void Awake()
    {
        healthCurrent = healthMax;
    }

    public void Damage(float amount)
    {
        healthCurrent -= amount;
        CheckIfDead();
    }

    public void Heal(float amount)
    {
        healthCurrent += amount;
        CapHealth();
    }

    public void SetHealth(float amount)
    {
        healthCurrent = amount;
        CheckIfDead();
        CapHealth();
    }

    // Check if the health has run out, and if so, DIE!!!
    private void CheckIfDead()
    {
        if (healthCurrent <= 0f)
        {
            OnDied();
        }
    }

    // Cap the health.
    private void CapHealth()
    {
        if (healthCurrent > healthMax)
        {
            healthCurrent = healthMax;
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
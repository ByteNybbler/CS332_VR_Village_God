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
    [Tooltip("Optional meter component for the health to interface with.")]
    public Meter meter;

    public delegate void DiedHandler();
    public event DiedHandler Died;

    private void Start()
    {
        if (meter != null)
        {
            meter.SetInitialState(healthCurrent, healthMax);
        }
    }

    public void Damage(float amount)
    {
        healthCurrent -= amount;
        UpdateMeterValue();
        CheckIfDead();
    }

    public void Heal(float amount)
    {
        healthCurrent += amount;
        UpdateMeterValue();
        CapHealth();
    }

    public void SetHealth(float amount)
    {
        healthCurrent = amount;
        CapHealth();
        UpdateMeterValue();
        CheckIfDead();
    }

    public void FullHeal()
    {
        healthCurrent = healthMax;
        UpdateMeterValue();
    }

    public void SetMaxHealth(float amount)
    {
        healthMax = amount;
        CapHealth();
        UpdateMeterMaxValue();
        CheckIfDead();
    }

    public void AddMaxHealth(float amount)
    {
        healthMax += amount;
        CapHealth();
        UpdateMeterMaxValue();
        CheckIfDead();
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

    private void UpdateMeterValue()
    {
        if (meter != null)
        {
            meter.SetValue(healthCurrent);
        }
    }

    private void UpdateMeterMaxValue()
    {
        if (meter != null)
        {
            meter.SetMaxValue(healthMax);
        }
    }
}
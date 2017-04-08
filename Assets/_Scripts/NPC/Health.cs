// Author(s): Paul Calande
// Health script.
// Arguments for these member functions are intended to only ever be positive.

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
            meter.SetBothValues(healthCurrent, healthMax);
        }
    }

    public void Damage(float amount)
    {
        healthCurrent -= amount;
        UpdateMeterCurrentValue();
        CheckIfDead();
    }

    public void Heal(float amount)
    {
        healthCurrent += amount;
        CapHealth();
        UpdateMeterCurrentValue();
    }

    public void SetHealth(float amount)
    {
        healthCurrent = amount;
        CapHealth();
        UpdateMeterCurrentValue();
        CheckIfDead();
    }

    // Restore the current health to the max health.
    public void FullHeal()
    {
        healthCurrent = healthMax;
        UpdateMeterCurrentValue();
    }

    // Set the health to 0, causing death.
    public void Die()
    {
        healthCurrent = 0f;
        UpdateMeterCurrentValue();
        OnDied();
    }

    public void SetMaxHealth(float amount)
    {
        healthMax = amount;
        CapHealth();
        UpdateMeterBothValues();
        CheckIfDead();
    }

    public void AddMaxHealth(float amount)
    {
        healthMax += amount;
        UpdateMeterMaxValue();
    }

    public void SubtractMaxHealth(float amount)
    {
        healthMax -= amount;
        CapHealth();
        UpdateMeterBothValues();
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

    private void UpdateMeterCurrentValue()
    {
        if (meter != null)
        {
            meter.SetCurrentValue(healthCurrent);
        }
    }

    private void UpdateMeterMaxValue()
    {
        if (meter != null)
        {
            meter.SetMaxValue(healthMax);
        }
    }

    private void UpdateMeterBothValues()
    {
        meter.SetBothValues(healthCurrent, healthMax);
    }
}
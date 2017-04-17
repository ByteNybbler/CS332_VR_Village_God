// Author(s): Paul Calande
// Health script.
// Arguments for these member functions are intended to only ever be positive.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The maximum health the object can have.")]
    private float healthMax;
    [SerializeField]
    [Tooltip("The current health the object has.")]
    private float healthCurrent;

    public delegate void DiedHandler();
    public event DiedHandler Died;
    public delegate void HealedHandler(float amount);
    public event HealedHandler Healed;
    public delegate void DamagedHandler(float amount);
    public event DamagedHandler Damaged;
    public delegate void MaxHealthAddedHandler(float amount);
    public event MaxHealthAddedHandler MaxHealthAdded;
    public delegate void MaxHealthSubtractedHandler(float amount);
    public event MaxHealthSubtractedHandler MaxHealthSubtracted;
    public delegate void CurrentHealthChangedHandler(float newHealthCurrent);
    public event CurrentHealthChangedHandler CurrentHealthChanged;
    public delegate void MaxHealthChangedHandler(float newHealthMax);
    public event MaxHealthChangedHandler MaxHealthChanged;

    public float GetCurrentHealth()
    {
        return healthCurrent;
    }

    public float GetMaxHealth()
    {
        return healthMax;
    }

    public void Damage(float amount)
    {
        if (!IsDead())
        {
            amount = Mathf.Min(amount, healthCurrent);
            healthCurrent -= amount;
            OnDamaged(amount);
            OnCurrentHealthChanged(healthCurrent);
        }
        if (IsDead())
        {
            OnDied();
        }
    }

    public void Heal(float amount)
    {
        if (!IsHealthFull())
        {
            amount = Mathf.Min(amount, healthMax - healthCurrent);
            healthCurrent += amount;
            OnHealed(amount);
            OnCurrentHealthChanged(healthCurrent);
        }
    }

    public void SetHealth(float amount)
    {
        if (amount > healthCurrent)
        {
            Heal(amount - healthCurrent);
        }
        if (amount < healthCurrent)
        {
            Damage(healthCurrent - amount);
        }
    }

    // Restore the current health to the max health.
    public void FullHeal()
    {
        Heal(healthMax - healthCurrent);
    }

    // Effectively set the health to 0, causing death.
    public void Die()
    {
        Damage(healthCurrent);
    }

    public void AddMaxHealth(float amount)
    {
        healthMax += amount;
        OnMaxHealthAdded(amount);
        OnMaxHealthChanged(healthMax);
    }

    public void SubtractMaxHealth(float amount)
    {
        healthMax -= amount;
        OnMaxHealthSubtracted(amount);
        OnMaxHealthChanged(healthMax);
        if (healthCurrent > healthMax)
        {
            Damage(healthCurrent - healthMax);
        }
    }

    public void SetMaxHealth(float amount)
    {
        if (amount > healthMax)
        {
            AddMaxHealth(amount - healthMax);
        }
        if (amount < healthMax)
        {
            SubtractMaxHealth(healthMax - amount);
        }
    }

    public bool IsHealthFull()
    {
        return (healthCurrent == healthMax);
    }

    // Returns true if there's no health left.
    private bool IsDead()
    {
        return (healthCurrent <= 0f);
    }

    // Event invocations.
    private void OnDied()
    {
        if (Died != null)
        {
            Died();
        }
    }
    private void OnHealed(float amount)
    {
        if (Healed != null)
        {
            Healed(amount);
        }
    }
    private void OnDamaged(float amount)
    {
        if (Damaged != null)
        {
            Damaged(amount);
        }
    }
    private void OnMaxHealthAdded(float amount)
    {
        if (MaxHealthAdded != null)
        {
            MaxHealthAdded(amount);
        }
    }
    private void OnMaxHealthSubtracted(float amount)
    {
        if (MaxHealthSubtracted != null)
        {
            MaxHealthSubtracted(amount);
        }
    }
    private void OnCurrentHealthChanged(float newHealthCurrent)
    {
        if (CurrentHealthChanged != null)
        {
            CurrentHealthChanged(newHealthCurrent);
        }
    }
    private void OnMaxHealthChanged(float newHealthMax)
    {
        if (MaxHealthChanged != null)
        {
            MaxHealthChanged(newHealthMax);
        }
    }
}
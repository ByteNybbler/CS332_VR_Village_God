// Author(s): Paul Calande
// Health script.
// Arguments for these member functions are intended to only ever be positive.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // The "type" of the healing/damage of changes in health.
    // This has no effect on the functionality within this class.
    // However, you can use this information outside of this class via events.
    public enum Type
    {
        Null, Impact, Hunger, Crush, Liquid, Divine, Electricity
    }

    [SerializeField]
    [Tooltip("The maximum health the object can have.")]
    private int healthMax;
    [SerializeField]
    [Tooltip("The current health the object has.")]
    private int healthCurrent;

    public delegate void DiedHandler(Type type);
    public event DiedHandler Died;
    public delegate void HealedHandler(int amount, Type type);
    public event HealedHandler Healed;
    public delegate void DamagedHandler(int amount, Type type);
    public event DamagedHandler Damaged;
    public delegate void MaxHealthAddedHandler(int amount, Type type);
    public event MaxHealthAddedHandler MaxHealthAdded;
    public delegate void MaxHealthSubtractedHandler(int amount, Type type);
    public event MaxHealthSubtractedHandler MaxHealthSubtracted;
    public delegate void CurrentHealthChangedHandler(int newHealthCurrent, Type type);
    public event CurrentHealthChangedHandler CurrentHealthChanged;
    public delegate void MaxHealthChangedHandler(int newHealthMax, Type type);
    public event MaxHealthChangedHandler MaxHealthChanged;

    public int GetCurrentHealth()
    {
        return healthCurrent;
    }

    public int GetMaxHealth()
    {
        return healthMax;
    }

    public void Damage(int amount, Type type)
    {
        if (!IsDead() && amount != 0)
        {
            amount = Mathf.Min(amount, healthCurrent);
            healthCurrent -= amount;
            OnDamaged(amount, type);
            OnCurrentHealthChanged(healthCurrent, type);
            if (IsDead())
            {
                OnDied(type);
            }
        }
    }

    public void Heal(int amount, Type type)
    {
        if (!IsHealthFull() && amount != 0)
        {
            amount = Mathf.Min(amount, healthMax - healthCurrent);
            healthCurrent += amount;
            OnHealed(amount, type);
            OnCurrentHealthChanged(healthCurrent, type);
        }
    }

    public void SetHealth(int amount, Type type)
    {
        if (amount > healthCurrent)
        {
            Heal(amount - healthCurrent, type);
        }
        if (amount < healthCurrent)
        {
            Damage(healthCurrent - amount, type);
        }
    }

    // Restore the current health to the max health.
    public void FullHeal(Type type)
    {
        Heal(healthMax - healthCurrent, type);
    }

    // Effectively set the health to 0, causing death.
    public void Die(Type type)
    {
        Damage(healthCurrent, type);
    }

    public void AddMaxHealth(int amount, Type type)
    {
        healthMax += amount;
        OnMaxHealthAdded(amount, type);
        OnMaxHealthChanged(healthMax, type);
    }

    public void SubtractMaxHealth(int amount, Type type)
    {
        healthMax -= amount;
        OnMaxHealthSubtracted(amount, type);
        OnMaxHealthChanged(healthMax, type);
        if (healthCurrent > healthMax)
        {
            Damage(healthCurrent - healthMax, type);
        }
    }

    public void SetMaxHealth(int amount, Type type)
    {
        if (amount > healthMax)
        {
            AddMaxHealth(amount - healthMax, type);
        }
        if (amount < healthMax)
        {
            SubtractMaxHealth(healthMax - amount, type);
        }
    }

    public bool IsHealthFull()
    {
        return (healthCurrent == healthMax);
    }

    // Returns true if there's no health left.
    public bool IsDead()
    {
        return (healthCurrent <= 0);
    }

    // Event invocations.
    private void OnDied(Type type)
    {
        if (Died != null)
        {
            Died(type);
        }
    }
    private void OnHealed(int amount, Type type)
    {
        if (Healed != null)
        {
            Healed(amount, type);
        }
    }
    private void OnDamaged(int amount, Type type)
    {
        if (Damaged != null)
        {
            Damaged(amount, type);
        }
    }
    private void OnMaxHealthAdded(int amount, Type type)
    {
        if (MaxHealthAdded != null)
        {
            MaxHealthAdded(amount, type);
        }
    }
    private void OnMaxHealthSubtracted(int amount, Type type)
    {
        if (MaxHealthSubtracted != null)
        {
            MaxHealthSubtracted(amount, type);
        }
    }
    private void OnCurrentHealthChanged(int newHealthCurrent, Type type)
    {
        if (CurrentHealthChanged != null)
        {
            CurrentHealthChanged(newHealthCurrent, type);
        }
    }
    private void OnMaxHealthChanged(int newHealthMax, Type type)
    {
        if (MaxHealthChanged != null)
        {
            MaxHealthChanged(newHealthMax, type);
        }
    }
}
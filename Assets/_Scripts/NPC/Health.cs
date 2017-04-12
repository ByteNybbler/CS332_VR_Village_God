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
    public delegate void HealedHandler(float amount);
    public event HealedHandler Healed;
    public delegate void DamagedHandler(float amount);
    public event DamagedHandler Damaged;
    public delegate void MaxHealthAddedHandler(float amount);
    public event MaxHealthAddedHandler MaxHealthAdded;
    public delegate void MaxHealthSubtractedHandler(float amount);
    public event MaxHealthSubtractedHandler MaxHealthSubtracted;

    // A dictionary of running coroutines related to trigger colliders
    // that the GameObject is currently inside of.
    private Dictionary<Collider, Coroutine> triggerTimers = new Dictionary<Collider, Coroutine>();
    // A list of HealthTriggers that the GameObject is currently inside of, used for assistance
    // with event subscriptions.
    private List<HealthTrigger> healthTriggers = new List<HealthTrigger>();

    private void Start()
    {
        if (meter != null)
        {
            meter.SetBothValues(healthCurrent, healthMax);
        }
    }

    public void Damage(float amount)
    {
        amount = Mathf.Min(amount, healthCurrent);
        healthCurrent -= amount;
        OnDamaged(amount);
        UpdateMeterCurrentValue();
        CheckIfDead();
    }

    public void Heal(float amount)
    {
        amount = Mathf.Min(amount, healthMax - healthCurrent);
        healthCurrent += amount;
        OnHealed(amount);
        UpdateMeterCurrentValue();
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
        if (!IsHealthFull())
        {
            OnHealed(healthMax - healthCurrent);
            healthCurrent = healthMax;
            UpdateMeterCurrentValue();
        }
    }

    // Set the health to 0, causing death.
    public void Die()
    {
        OnDamaged(healthCurrent);
        healthCurrent = 0f;
        UpdateMeterCurrentValue();
        OnDied();
    }

    public void AddMaxHealth(float amount)
    {
        healthMax += amount;
        OnMaxHealthAdded(amount);
        UpdateMeterMaxValue();
    }

    public void SubtractMaxHealth(float amount)
    {
        healthMax -= amount;
        OnMaxHealthSubtracted(amount);
        if (healthCurrent > healthMax)
        {
            Damage(healthCurrent - healthMax);
        }
        UpdateMeterMaxValue();
        CheckIfDead();
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

    // Check if the health has run out, and if so, DIE!!!
    private void CheckIfDead()
    {
        if (healthCurrent <= 0f)
        {
            OnDied();
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

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Health: Entered trigger.");
        HealthTrigger ht = other.GetComponent<HealthTrigger>();
        // If the other object has a HealthTrigger...
        if (ht != null)
        {
            // Start its coroutine and add it to the dictionary and list.
            Coroutine c;
            ht.Disabled += HealthTrigger_Disabled;
            healthTriggers.Add(ht);
            switch (ht.type)
            {
                case HealthTrigger.Type.damage:
                    c = StartCoroutine(TriggerHurt(ht.amount, ht.timeBetweenFires));
                    break;
                case HealthTrigger.Type.heal:
                    c = StartCoroutine(TriggerHeal(ht.amount, ht.timeBetweenFires));
                    break;
                case HealthTrigger.Type.setHealth:
                default:
                    c = StartCoroutine(TriggerSetHealth(ht.amount, ht.timeBetweenFires));
                    break;
            }
            triggerTimers.Add(other, c);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Health: OnTriggerExit");
        // Upon exiting a health trigger, remove it from the dictionary and stop its coroutine.
        if (other.GetComponent<HealthTrigger>() != null)
        {
            TriggerRemove(other);
        }
    }

    private void TriggerRemove(Collider other)
    {
        Coroutine c;
        if (triggerTimers.TryGetValue(other, out c))
        {
            //Debug.Log("TriggerRemove: Health component: " + this);
            StopCoroutine(c);
            triggerTimers.Remove(other);
            HealthTrigger ht = other.GetComponent<HealthTrigger>();
            ht.Disabled -= HealthTrigger_Disabled;
            healthTriggers.Remove(ht);
        }
    }

    private void OnEnable()
    {
        foreach (HealthTrigger ht in healthTriggers)
        {
            ht.Disabled += HealthTrigger_Disabled;
        }
    }

    private void OnDisable()
    {
        foreach (HealthTrigger ht in healthTriggers)
        {
            ht.Disabled -= HealthTrigger_Disabled;
        }
    }

    IEnumerator TriggerHurt(float amount, float timeBetweenFires)
    {
        while (true)
        {
            //Debug.Log("TriggerHurt timer: " + timeBetweenFires * Time.timeScale);
            Damage(amount);
            yield return new WaitForSeconds(timeBetweenFires);
        }
    }
    IEnumerator TriggerHeal(float amount, float timeBetweenFires)
    {
        while (true)
        {
            Heal(amount);
            yield return new WaitForSeconds(timeBetweenFires);
        }
    }
    IEnumerator TriggerSetHealth(float amount, float timeBetweenFires)
    {
        while (true)
        {
            SetHealth(amount);
            yield return new WaitForSeconds(timeBetweenFires);
        }
    }

    private void HealthTrigger_Disabled(Collider col)
    {
        TriggerRemove(col);
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
}
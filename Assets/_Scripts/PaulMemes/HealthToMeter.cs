// Author(s): Paul Calande
// Component for attaching a health quantity to a meter.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthToMeter : MonoBehaviour
{
    [Tooltip("The health component to interface with.")]
    public Health compHealth;
    [Tooltip("The meter component to interface with.")]
    public Meter compMeter;

    private void Start()
    {
        compMeter.SetBothValues(compHealth.GetCurrentHealth(), compHealth.GetMaxHealth());
    }

    private void OnEnable()
    {
        compHealth.CurrentHealthChanged += Health_CurrentHealthChanged;
        compHealth.MaxHealthChanged += Health_MaxHealthChanged;
    }
    private void OnDisable()
    {
        compHealth.CurrentHealthChanged -= Health_CurrentHealthChanged;
        compHealth.MaxHealthChanged -= Health_MaxHealthChanged;
    }

    private void Health_CurrentHealthChanged(int newHealthCurrent)
    {    
        compMeter.SetCurrentValue(newHealthCurrent);
    }

    private void Health_MaxHealthChanged(int newHealthMax)
    {
        compMeter.SetMaxValue(newHealthMax);
    }
}
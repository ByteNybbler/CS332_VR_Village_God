// Author(s): Paul Calande
// NPC health script that handles rising text functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [Tooltip("Rising text creator for healing.")]
    public RisingTextCreator rtcHealed;
    [Tooltip("Rising text creator for damage.")]
    public RisingTextCreator rtcDamaged;
    [Tooltip("The string that is the icon for HP.")]
    public string hpString = "HP";
    [Tooltip("How big the health change must be for the rising text to be displayed.")]
    public float minimumHealthChangeForText = 0.5f;

    public delegate void DiedHandler();
    public event DiedHandler Died;

    // Component references.
    private Health compHealth;

    private void Awake()
    {
        compHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        compHealth.Damaged += Health_Damaged;
        compHealth.Healed += Health_Healed;
        compHealth.Died += Health_Died;
    }
    private void OnDisable()
    {
        compHealth.Damaged -= Health_Damaged;
        compHealth.Healed -= Health_Healed;
        compHealth.Died -= Health_Died;
    }

    private void Health_Damaged(float amount)
    {
        if (amount > minimumHealthChangeForText)
        {
            rtcDamaged.message = "-" + Mathf.CeilToInt(amount) + " " + hpString;
            rtcDamaged.CreateRisingText(transform.position);
        }
    }

    private void Health_Healed(float amount)
    {
        if (amount > minimumHealthChangeForText)
        {
            rtcHealed.message = "+" + Mathf.CeilToInt(amount) + " " + hpString;
            rtcHealed.CreateRisingText(transform.position);
        }
    }

    private void Health_Died()
    {
        OnDied();
        Destroy(gameObject);
    }

    private void OnDied()
    {
        if (Died != null)
        {
            Died();
        }
    }
}
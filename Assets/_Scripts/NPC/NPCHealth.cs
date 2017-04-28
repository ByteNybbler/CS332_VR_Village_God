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
    [Tooltip("The audio clip that plays when the NPC hits the water.")]
    public AudioClip soundWaterImpact;

    public delegate void DiedHandler();
    public event DiedHandler Died;

    // Component references.
    private Health compHealth;

    private void Awake()
    {
        compHealth = GetComponent<Health>();
        compHealth.Damaged += Health_Damaged;
        compHealth.Healed += Health_Healed;
        compHealth.Died += Health_Died;
    }

    private void OnDestroy()
    {
        compHealth.Damaged -= Health_Damaged;
        compHealth.Healed -= Health_Healed;
        compHealth.Died -= Health_Died;
    }

    // Make the NPC die.
    public void Die()
    {
        OnDied();
        Destroy(gameObject);
    }

    private void Health_Damaged(int amount, Health.Type type)
    {
        if (type != Health.Type.Null && type != Health.Type.Hunger)
        {
            rtcDamaged.message = "-" + Mathf.CeilToInt(amount) + " " + hpString;
            rtcDamaged.CreateRisingText(transform.position);
            if (type == Health.Type.Liquid)
            {
                // Use global audio since the NPC is likely being destroyed.
                GlobalAudio.source.PlayOneShot(soundWaterImpact);
            }
        }
    }

    private void Health_Healed(int amount, Health.Type type)
    {
        if (type != Health.Type.Null)
        {
            rtcHealed.message = "+" + Mathf.CeilToInt(amount) + " " + hpString;
            rtcHealed.CreateRisingText(transform.position);
        }
    }

    private void Health_Died(Health.Type type)
    {
        Die();
    }

    private void OnDied()
    {
        if (Died != null)
        {
            Died();
        }
    }
}
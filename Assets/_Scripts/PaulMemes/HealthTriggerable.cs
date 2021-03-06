﻿// Author(s): Paul Calande
// Script that allows the Health component to be affected by HealthTrigger objects.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTriggerable : MonoBehaviour
{
    // A dictionary of running coroutines related to trigger colliders
    // that the GameObject is currently inside of.
    private Dictionary<Collider, Coroutine> triggerTimers = new Dictionary<Collider, Coroutine>();
    // A list of HealthTriggers that the GameObject is currently inside of, used for assistance
    // with event subscriptions.
    private List<HealthTrigger> healthTriggers = new List<HealthTrigger>();

    // Component references.
    private Health compHealth;

    private void Awake()
    {
        compHealth = GetComponent<Health>();
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
            switch (ht.triggerType)
            {
                case HealthTrigger.Type.Damage:
                    c = StartCoroutine(TriggerHurt(ht.amount, ht.timeBetweenFires, ht.healthType));
                    break;
                case HealthTrigger.Type.Heal:
                    c = StartCoroutine(TriggerHeal(ht.amount, ht.timeBetweenFires, ht.healthType));
                    break;
                case HealthTrigger.Type.SetHealth:
                default:
                    c = StartCoroutine(TriggerSetHealth(ht.amount, ht.timeBetweenFires, ht.healthType));
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

    IEnumerator TriggerHurt(int amount, float timeBetweenFires, Health.Type healthType)
    {
        while (true)
        {
            //Debug.Log("TriggerHurt timer: " + timeBetweenFires * Time.timeScale);
            compHealth.Damage(amount, healthType);
            yield return new WaitForSeconds(timeBetweenFires);
        }
    }
    IEnumerator TriggerHeal(int amount, float timeBetweenFires, Health.Type healthType)
    {
        while (true)
        {
            compHealth.Heal(amount, healthType);
            yield return new WaitForSeconds(timeBetweenFires);
        }
    }
    IEnumerator TriggerSetHealth(int amount, float timeBetweenFires, Health.Type healthType)
    {
        while (true)
        {
            compHealth.SetHealth(amount, healthType);
            yield return new WaitForSeconds(timeBetweenFires);
        }
    }

    private void HealthTrigger_Disabled(Collider col)
    {
        TriggerRemove(col);
    }
}
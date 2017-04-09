// Author(s): Paul Calande
// Script that allows a GameObject with a Health component to be affected by death triggers.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerable : MonoBehaviour
{
    // Component references.
    private Health compHealth;

    private void Awake()
    {
        compHealth = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathTrigger")
        {
            compHealth.Die();
        }
    }
}
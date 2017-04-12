// Author(s): Paul Calande
// Ball of Death script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOfDeath : MonoBehaviour
{
    [Tooltip("Seconds between each damage recalculation.")]
    public float damageUpdateFrequency = 0.2f;

    // Component references.
    private HealthTrigger ht;
    private Rigidbody rb;

    private void Awake()
    {
        ht = GetComponent<HealthTrigger>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(CalculateDamage());
    }

    // Calculate the damage based on how quickly the ball of death is moving.
    IEnumerator CalculateDamage()
    {
        while (true)
        {
            ht.amount = Mathf.Floor(rb.velocity.magnitude);
            yield return new WaitForSeconds(damageUpdateFrequency);
        }
    }
}
// Author(s): Paul Calande
// Ball of Death script.

// Comment out the following line to stop the Ball of Death from constantly printing its damage to the console.
//#define BOD_PRINT_DAMAGE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOfDeath : MonoBehaviour
{
    [Tooltip("How much the Ball of Death's damage is multiplied by.")]
    public float damageMultiplier = 1f;

    // Component references.
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

#if BOD_PRINT_DAMAGE
    private void Update()
    {
        Debug.Log("Ball of Death Update Damage: " + CalculateDamage());
    }
#endif

    // Calculate the damage that the ball does based on its speed.
    private int CalculateDamage()
    {
        return (int)(rb.velocity.magnitude * damageMultiplier);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Ball of Death collided with " + collision.gameObject.name);
        if (collision.gameObject.tag == "NPC")
        {
            //Debug.Log("Damage upon NPC collision: " + CalculateDamage());
            Health otherHealth = collision.gameObject.GetComponent<Health>();
            otherHealth.Damage(CalculateDamage());
        }
    }
}
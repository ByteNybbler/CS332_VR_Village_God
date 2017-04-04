// Author(s): Paul Calande
// Shrine script.

// Comment out the following line to prevent the shrine's points from being printed to the console.
#define DEBUG_SHRINE_POINTS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    [Tooltip("How many charge seconds it takes to gain 1 point.\n" +
    "One villager idling at the shrine for one second = one charge second.")]
    public float chargeSecondsPerPoint;
    [Tooltip("How many points the shrine currently has.\n" +
    "Change this quantity in the inspector to change how many points the shrine starts off with.")]
    public int points = 0;

    // The quantity of charge (in charge seconds) that the shrine has yet to convert into points.
    private float chargeSeconds;

    // Component references.
    RisingTextCreator rtc;

    private void Awake()
    {
        rtc = GetComponent<RisingTextCreator>();
    }

    // Use this public function to add charge seconds to the shrine.
    // rootPosition is the spawn position of the +1.
    public void IncreaseChargeSeconds(float amount, Vector3 rootPosition)
    {
        chargeSeconds += amount;
        while (chargeSeconds > chargeSecondsPerPoint)
        {
            chargeSeconds -= chargeSecondsPerPoint;
            int additionalPoints = 1;
            points += additionalPoints;
            // Instantiate the +1 canvas.
            rtc.message = "+" + additionalPoints + " faith";
            rtc.CreateRisingText(rootPosition);

#if DEBUG_SHRINE_POINTS
            Debug.Log("Shrine points: " + points);
#endif
        }
    }

    // This function is used for buying stuff with the shrine's points.
    // If the player has enough points, those points will be spent and true will be returned.
    // Otherwise, return false.
    public bool SpendPoints(int amount)
    {
        if (points >= amount)
        {
            points -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
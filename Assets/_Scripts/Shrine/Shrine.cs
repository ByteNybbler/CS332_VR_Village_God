// Author(s): Paul Calande
// Shrine script.

// Comment out the following line to prevent the shrine's points from being printed to the console.
#define DEBUG_SHRINE_POINTS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    // How many charge seconds it takes to gain 1 point.
    // One villager idling at the shrine for one second = one charge second.
    public float chargeSecondsPerPoint;
    // How many points the shrine currently has.
    // Change this quantity in the inspector to change how many points the shrine starts off with.
    public int points = 0;

    // The quantity of charge (in charge seconds) that the shrine has yet to convert into points.
    private float chargeSeconds;

    // Use this public function to add charge seconds to the shrine.
    public void IncreaseChargeSeconds(float amount)
    {
        chargeSeconds += amount;
        while (chargeSeconds > chargeSecondsPerPoint)
        {
            chargeSeconds -= chargeSecondsPerPoint;
            points += 1;

#if DEBUG_SHRINE_POINTS
            Debug.Log("Shrine points: " + points);
#endif
        }
    }
}
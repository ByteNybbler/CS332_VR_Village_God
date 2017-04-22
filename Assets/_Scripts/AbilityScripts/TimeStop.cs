// Author(s): Paul Calande
// Script for the ability to stop time. ZA WARUDO!!!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : Ability
{
    [Tooltip("How many seconds the timestop lasts.")]
    public float timeStopLength = 10f;

    public override void PointerLocationAbility(Vector3 location)
    {
        // Stop time!
        car.timeController.StopTime(timeStopLength);
    }

    // Only use this ability if time isn't stopped already.
    public override bool AdditionalPointerChecks(Vector3 location)
    {
        return !car.timeController.IsTimeStopped();
    }
}
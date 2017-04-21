// Author(s): Paul Calande
// Time scale component.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    // The scale of the time.
    public float scale = 1.0f;

    // Get the timescale itself.
    public float GetTimeScale()
    {
        return scale;
    }

    // Set the timescale.
    public void SetTimeScale(float amount)
    {
        scale = amount;
        // Adjust other components accordingly.
        // TODO
    }

    // Get the scaled version of delta time.
    public float GetTimePassed()
    {
        return Time.deltaTime * scale;
    }
}
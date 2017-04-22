// Author(s): Paul Calande
// Time scale component.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    // The scale of the time.
    private float scale = 1.0f;

    private void Start()
    {
        // Record the default values of components.
    }

    // Get the timescale itself.
    public float GetTimeScale()
    {
        return scale;
    }

    // Set the timescale.
    public void SetTimeScale(float amount)
    {
        if (amount == 1f)
        {
            ResetTimeScale();
        }
        else
        {
            scale = amount;
            // Adjust other components accordingly.
            // TODO
        }
    }

    // Set the time scale back to its default, 1.
    public void ResetTimeScale()
    {
        scale = 1f;
        // Set components back to their default values.
        // TODO
    }

    // Get the scaled version of delta time.
    public float GetTimePassed()
    {
        return Time.deltaTime * scale;
    }
}
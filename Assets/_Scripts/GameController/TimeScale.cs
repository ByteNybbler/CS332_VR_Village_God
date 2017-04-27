// Author(s): Paul Calande
// Time scale component.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    // The scale of the time.
    private float scale = 1.0f;

    public delegate void TimeScaleChangedHandler(float timescale);
    public event TimeScaleChangedHandler Changed;
    public delegate void TimeScaleResetHandler();
    public event TimeScaleResetHandler Reset;

    // Pass a time scale from the passer to the receiver.
    public static void PassTimeScale(GameObject receiver, GameObject passer)
    {
        receiver.GetComponent<TimeScale>().SetTimeScale(passer.GetComponent<TimeScale>().GetTimeScale());
    }
    public static void PassTimeScale(GameObject receiver, TimeScale passer)
    {
        receiver.GetComponent<TimeScale>().SetTimeScale(passer.GetTimeScale());
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
            OnChanged(amount);
        }
    }

    // Set the time scale back to its default, 1.
    public void ResetTimeScale()
    {
        scale = 1f;
        OnReset();
    }

    // Get the scaled version of delta time.
    public float GetTimePassed()
    {
        return Time.deltaTime * scale;
    }

    // Event invocations.
    private void OnChanged(float timescale)
    {
        if (Changed != null)
        {
            Changed(timescale);
        }
    }
    private void OnReset()
    {
        if (Reset != null)
        {
            Reset();
        }
    }
}
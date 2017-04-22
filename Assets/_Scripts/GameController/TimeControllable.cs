// Author(s): Paul Calande
// Mediator class for time stop.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllable : MonoBehaviour
{
    [Tooltip("Reference to the time controller.")]
    public TimeController timeController;

    // Component references.
    private TimeScale ts;

    private void Awake()
    {
        ts = GetComponent<TimeScale>();
    }

    public void Start()
    {
        if (timeController != null)
        {
            timeController.TimeStopped += TimeController_TimeStopped;
            timeController.TimeResumed += TimeController_TimeResumed;
        }
    }

    private void OnDestroy()
    {
        if (timeController != null)
        {
            timeController.TimeStopped -= TimeController_TimeStopped;
            timeController.TimeResumed -= TimeController_TimeResumed;
        }
    }

    private void TimeController_TimeStopped()
    {
        ts.SetTimeScale(0f);
    }

    private void TimeController_TimeResumed()
    {
        ts.SetTimeScale(1f);
    }
}
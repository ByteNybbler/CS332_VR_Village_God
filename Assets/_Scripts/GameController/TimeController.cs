// Author(s): Paul Calande
// Controller for stopping time.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Tooltip("The audio source to use for the time stop sound effect.")]
    public AudioSource audioSource;
    [Tooltip("Audio clip for time stopping.")]
    public AudioClip soundTimeStop;
    [Tooltip("Audio clip for time resuming.")]
    public AudioClip soundTimeResume;

    public delegate void TimeStoppedHandler();
    public event TimeStoppedHandler TimeStopped;
    public delegate void TimeResumedHandler();
    public event TimeResumedHandler TimeResumed;

    // Timer for stopped time.
    private float timeStopTimer = 0f;
    // Whether time is currently stopped.
    private bool isTimeStopped = false;

    private void Update()
    {
        if (timeStopTimer > 0f)
        {
            timeStopTimer -= Time.deltaTime;
            if (timeStopTimer <= 0f)
            {
                ResumeTime();
            }
        }
    }

    // Stop time for a certain number of seconds.
    public void StopTime(float seconds)
    {
        audioSource.PlayOneShot(soundTimeStop);
        isTimeStopped = true;
        // Stop time!
        OnTimeStopped();
        // Prepare to resume time after a number of seconds.
        timeStopTimer = seconds;
    }

    private void ResumeTime()
    {
        audioSource.PlayOneShot(soundTimeResume);
        isTimeStopped = false;
        // Set time back into motion!
        OnTimeResumed();
    }

    // Return true if time is stopped.
    public bool IsTimeStopped()
    {
        return isTimeStopped;
    }

    // Event invocations.
    private void OnTimeStopped()
    {
        if (TimeStopped != null)
        {
            TimeStopped();
        }
    }
    private void OnTimeResumed()
    {
        if (TimeResumed != null)
        {
            TimeResumed();
        }
    }
}
// Author(s): Paul Calande
// Script for the ability to stop time. ZA WARUDO!!!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : Ability
{
    [Tooltip("How many seconds the timestop lasts.")]
    public float timeStopLength = 10f;
    [Tooltip("The audio source to use for the time stop sound effect.")]
    public AudioSource cAudioSource;
    [Tooltip("Audio clip for time stopping.")]
    public AudioClip soundTimeStop;
    [Tooltip("Audio clip for time resuming.")]
    public AudioClip soundTimeResume;

    //[Tooltip("Whether or not time is currently stopped.")]
    //public bool timeIsStopped = false;

    public delegate void TimeStoppedHandler();
    public event TimeStoppedHandler TimeStopped;
    public delegate void TimeResumedHandler();
    public event TimeResumedHandler TimeResumed;

    public override void PointerLocationAbility(Vector3 location)
    {
        cAudioSource.PlayOneShot(soundTimeStop);
        // Stop time!
        Time.timeScale = 0.00001f;
        OnTimeStopped();
        // Start the coroutine that will resume time after a while.
        StartCoroutine(StopTimeCountdown());
    }

    IEnumerator StopTimeCountdown()
    {
        yield return new WaitForSeconds(timeStopLength * Time.timeScale);
        ResumeTime();
    }

    private void ResumeTime()
    {
        cAudioSource.PlayOneShot(soundTimeResume);
        // Set time back into motion!
        Time.timeScale = 1f;
        OnTimeResumed();
    }

    private void OnDisable()
    {
        ResumeTime();
    }

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
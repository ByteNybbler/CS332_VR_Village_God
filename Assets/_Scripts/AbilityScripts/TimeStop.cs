﻿// Author(s): Paul Calande
// Script for the ability to stop time. ZA WARUDO!!!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [Tooltip("How many points the ability costs.")]
    public int cost;
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

    // Component references.
    private AbilityInterface cai;

    private void Awake()
    {
        cai = GetComponent<AbilityInterface>();
    }

    public void TimeAbility()
    {
        Vector3 location;
        if (cai.castRayLeftController.Cast(out location))
        {
            if (cai.shrine.SpendPoints(cost, location))
            {
                StopTime();
            }
        }
    }

    public void StopTime()
    {
        cAudioSource.PlayOneShot(soundTimeStop);
        // Stop time!
        Time.timeScale = 0.00001f;
        // Start the coroutine that will resume time after a while.
        StartCoroutine(StopTimeCountdown());
    }

    IEnumerator StopTimeCountdown()
    {
        yield return new WaitForSeconds(timeStopLength);
        ResumeTime();
    }

    private void ResumeTime()
    {
        cAudioSource.PlayOneShot(soundTimeResume);
        // Set time back into motion!
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        ResumeTime();
    }
}
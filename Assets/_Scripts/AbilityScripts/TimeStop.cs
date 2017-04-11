// Author(s): Paul Calande
// ZA WARUDO!!!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [Tooltip("How many points the ability costs.")]
    public int cost;
    [Tooltip("The audio source to use for the time stop sound effect.")]
    public AudioSource cAudioSource;

    // Component references.
    private AbilityInterface cai;

    private void Awake()
    {
        cai = GetComponent<AbilityInterface>();
    }

    public void StopTime()
    {
        Vector3 location;
        if (cai.castRayLeftController.Cast(out location))
        {
            if (cai.shrine.SpendPoints(cost, location))
            {
                cAudioSource.PlayOneShot(cAudioSource.clip);
            }
        }
    }
}
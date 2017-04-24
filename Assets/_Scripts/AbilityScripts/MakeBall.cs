// Author(s): Paul Calande
// Ability script for making a ball of death.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBall : Ability
{
    [Tooltip("Ball of Death prefab to instantiate.")]
    public GameObject prefabBallOfDeath;
    [Tooltip("The Audio Source to use to play sound.")]
    public AudioSource audioSource;
    [Tooltip("The Audio Clip to play.")]
    public AudioClip audioClip;

    public override void PointerLocationAbility(Vector3 location)
    {
        // Calculate the required y offset so that the Ball of Death doesn't spawn halfway into the ground.
        float yOffset = prefabBallOfDeath.transform.localScale.y * 0.5f;
        Vector3 creationLocation = new Vector3(location.x, location.y + yOffset, location.z);
        // Instantiate the Ball of Death accordingly.
        Instantiate(prefabBallOfDeath, creationLocation, Quaternion.identity);
    }
}
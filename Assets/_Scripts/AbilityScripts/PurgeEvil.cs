// Author(s): Paul Calande
// Ability for instantly killing all enemies in the scene.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgeEvil : Ability
{
    [Tooltip("The Audio Source to use to play sound.")]
    public AudioSource audioSource;
    [Tooltip("The Audio Clip to play.")]
    public AudioClip audioClip;

    public override void PointerLocationAbility(Vector3 location)
    {
        audioSource.PlayOneShot(audioClip);
        car.gameController.enemyController.KillAllEnemies();
    }
}
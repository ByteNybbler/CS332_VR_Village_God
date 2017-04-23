// Author(s): Paul Calande
// Ability that fully heals all villagers in the scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerHealAll : Ability
{
    [Tooltip("The Audio Source to use to play sound.")]
    public AudioSource audioSource;
    [Tooltip("The Audio Clip to play.")]
    public AudioClip audioClip;

    public override void PointerLocationAbility(Vector3 location)
    {
        audioSource.PlayOneShot(audioClip);
        car.gameController.village.FullHealAllVillagers(Health.Type.Divine);
    }
}
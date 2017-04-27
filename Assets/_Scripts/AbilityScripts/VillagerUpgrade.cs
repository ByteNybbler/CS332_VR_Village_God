// Author(s): Paul Calande
// Ability that can be used to upgrade villagers.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerUpgrade : Ability
{
    [Tooltip("The Audio Source to use to play sound.")]
    public AudioSource audioSource;
    [Tooltip("The Audio Clip to play.")]
    public AudioClip audioClip;
    [Tooltip("How much the cost of the ability is multiplied by per use.")]
    public float costMultiplierPerUse = 1.1f;
    [Tooltip("How close the villager must be to the pointer to be affected.")]
    public float range;

    // The villager being pointed at.
    private VillagerStatus target;

    public override void PointerLocationAbility(Vector3 location)
    {
        audioSource.PlayOneShot(audioClip);
        target.Upgrade();
        cost = Mathf.CeilToInt(cost * costMultiplierPerUse);
    }

    // Return true if a villager is close enough to a pointer.
    public override bool AdditionalPointerChecks(Vector3 location)
    {
        target = car.gameController.village.GetClosestVillager(location, range);
        return (target != null);
    }
}
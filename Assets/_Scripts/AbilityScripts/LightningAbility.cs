// Author(s): Paul Calande, Hunter Golden
// Lightning ability script that interfaces with a lightning bolt.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAbility : MonoBehaviour
{
    [Tooltip("Reference to the LightningBoltScript for the lightning bolt itself.")]
    public DigitalRuby.LightningBolt.LightningBoltScript compLightning;
    [Tooltip("The cost in faith points of the lightning.")]
    public int cost = 5;
    [Tooltip("Reference to the SoundArray component.")]
    public SoundArray compSoundArray;
    [Tooltip("The particle effect to be created on the lightning bolt end location.")]
    public ParticleSystem lightningBlast;
    [Tooltip("The object that is created at the lightning's end point.")]
    public GameObject areaOfEffect;
    [Tooltip("How high the lightning extends above the end point.")]
    public float lightningHeight = 100f;

    // Component references.
    private AbilityInterface cai;

    private void Awake()
    {
        cai = GetComponent<AbilityInterface>();
    }

    public void ShootLightning()
    {
        Vector3 location;
        if (cai.castRayRightController.Cast(out location))
        {
            if (cai.shrine.SpendPoints(cost, location))
            {
                // Calculate the lightning's start and end points.
                Vector3 start, end;
                Vector3 additionalY = new Vector3(0f, lightningHeight, 0f);
                start = location + additionalY;
                end = location;

                // Interface with the third-party lightning script to create the lightning itself.
                compLightning.StartPosition = start;
                compLightning.EndPosition = end;
                compLightning.Trigger();

                // Play a lightning sound.
                compSoundArray.PlayRandomSound();

                // Instantiate the particles and area of effect object.
                Instantiate(lightningBlast, end, Quaternion.identity);
                Instantiate(areaOfEffect, end, Quaternion.identity);
            }
        }
    }
}
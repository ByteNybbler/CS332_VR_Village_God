// Author(s): Hunter Golden, Paul Calande
// Sets the color of the water particles.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColor : MonoBehaviour
{
	private void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        //ps.startColor = new Color (1, 0, 1, .5f);
        ParticleSystem.MainModule dupe = ps.main;
        dupe.startColor = new Color(1f, 0f, 1f, .5f);
    }
}
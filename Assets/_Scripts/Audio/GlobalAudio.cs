// Author(s): Paul Calande
// Global audio class. Intended to be used with 2D sound.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundArray : MonoBehaviour
{
	public AudioSource compAudio;
	public AudioClip[] buttonSounds;

	public void PlayRandomSound()
    {
		int random = Random.Range(0,buttonSounds.Length);
		compAudio.PlayOneShot(buttonSounds[random]);
	}
}
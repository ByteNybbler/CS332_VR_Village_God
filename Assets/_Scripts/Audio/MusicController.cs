// Author(s): Paul Calande
// Mountain God VR music controller class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Tooltip("Reference to the AudioSource to use.")]
    public AudioSource audioSource;
    [Tooltip("Music that plays when the village isn't under attack.")]
    public AudioClip musicPeaceful;
    [Tooltip("Music that plays when the village is under attack.")]
    public AudioClip musicBattle;
    [Tooltip("Reference to the village instance.")]
    public Village village;

    private void Start()
    {
        village.AttackStarted += Village_AttackStarted;
        village.AttackEnded += Village_AttackEnded;

        ChangeMusic(false);
    }

    private void OnDestroy()
    {
        if (village != null)
        {
            village.AttackStarted -= Village_AttackStarted;
            village.AttackEnded -= Village_AttackEnded;
        }
    }

    public void ChangeMusic(bool isVillageBeingAttacked)
    {
        audioSource.Stop();
        if (isVillageBeingAttacked == false)
        {
            audioSource.clip = musicPeaceful;
        }
        else
        {
            audioSource.clip = musicBattle;
        }
        audioSource.Play();
    }

    private void Village_AttackStarted()
    {
        ChangeMusic(true);
    }

    private void Village_AttackEnded()
    {
        ChangeMusic(false);
    }
}
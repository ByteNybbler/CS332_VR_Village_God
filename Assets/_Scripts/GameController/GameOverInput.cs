// Author(s): Paul Calande
// Game over input class.

// Comment out the following line to prevent debug input.
//#define GAMEOVERINPUT_DEBUG_INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverInput : MonoBehaviour
{
    [Tooltip("Reference to the image fade.")]
    public ImageFade imageFade;
    [Tooltip("Reference to the controller events component of the left controller.")]
    public VRTK.VRTK_ControllerEvents leftControllerEvents;
    [Tooltip("Reference to the controller events component of the right controller.")]
    public VRTK.VRTK_ControllerEvents rightControllerEvents;
    [Tooltip("Reference to the waves text.")]
    public Text textWaves;
    [Tooltip("Reference to the enemies killed text.")]
    public Text textEnemiesKilled;

    private void Start()
    {
        leftControllerEvents.TriggerPressed += ControllerEvents_TriggerPressed;
        leftControllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        rightControllerEvents.TriggerPressed += ControllerEvents_TriggerPressed;
        rightControllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
    }

    private void OnDestroy()
    {
        if (leftControllerEvents != null)
        {
            leftControllerEvents.TriggerPressed -= ControllerEvents_TriggerPressed;
            leftControllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
        }
        if (rightControllerEvents != null)
        {
            rightControllerEvents.TriggerPressed -= ControllerEvents_TriggerPressed;
            rightControllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
        }
    }

    private void FadeIn()
    {
        imageFade.state = ImageFade.State.FadeIn;
    }

    private void FadeOut()
    {
        imageFade.state = ImageFade.State.FadeOut;
    }

    private void ControllerEvents_TriggerPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        FadeIn();
    }

    private void ControllerEvents_TriggerReleased(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        FadeOut();
    }

#if GAMEOVERINPUT_DEBUG_INPUT
    private void Update()
    {
        // For testing the fade in and fade out functionality.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FadeIn();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FadeOut();
        }
    }
#endif
}
// Author(s): Paul Calande
// Game over input class.

// Comment out the following line to prevent debug input.
//#define GAMEOVERINPUT_DEBUG_INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverInput : MonoBehaviour
{
    [Tooltip("Reference to the image fade.")]
    public ImageFade imageFade;
    [Tooltip("Reference to the controller events component to detect controller inputs.")]
    public VRTK.VRTK_ControllerEvents controllerEvents;

    private void Start()
    {
        controllerEvents.TriggerPressed += ControllerEvents_TriggerPressed;
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
    }

    private void OnDestroy()
    {
        if (controllerEvents != null)
        {
            controllerEvents.TriggerPressed -= ControllerEvents_TriggerPressed;
            controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
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
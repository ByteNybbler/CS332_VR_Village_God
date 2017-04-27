// Author(s): Paul Calande
// Game over input class.

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
        controllerEvents.TriggerPressed -= ControllerEvents_TriggerPressed;
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
    }

    private void ControllerEvents_TriggerPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        imageFade.state = ImageFade.State.FadeIn;
    }

    private void ControllerEvents_TriggerReleased(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        imageFade.state = ImageFade.State.FadeOut;
    }
}
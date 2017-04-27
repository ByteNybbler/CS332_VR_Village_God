// Author(s): Paul Calande
// Disables a component on Start.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRendererOnStart : MonoBehaviour
{
    [Tooltip("The component to disable.")]
    public Renderer component;

    private void Start()
    {
        component.enabled = false;
    }
}
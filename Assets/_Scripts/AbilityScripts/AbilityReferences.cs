// Author(s): Paul Calande
// References to various useful components that abilities can interface with.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityReferences : MonoBehaviour
{
    [Tooltip("Reference to the left controller's CastRay component.")]
    public CastRay castRayLeftController;
    [Tooltip("Reference to the right controller's CastRay component.")]
    public CastRay castRayRightController;
    [Tooltip("Shrine component reference.")]
    public Shrine shrine;
    [Tooltip("Game Controller component reference.")]
    public GameController gameController;
}
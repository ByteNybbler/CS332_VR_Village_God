// Author(s): Paul Calande
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInterface : MonoBehaviour
{
    [Tooltip("Reference to the left controller's CastRay component.")]
    public CastRay castRayLeftController;
    [Tooltip("Reference to the right controller's CastRay component.")]
    public CastRay castRayRightController;
    [Tooltip("Shrine component reference.")]
    public Shrine shrine;
}
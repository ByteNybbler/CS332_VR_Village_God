// Author(s): Paul Calande
// Press B to gain tons of shrine points.
// Disable this script component when it's not being used for testing purposes.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressBToCheat : MonoBehaviour
{
    // Component references.
    private AbilityReferences car;

    private void Awake()
    {
        car = GetComponent<AbilityReferences>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            car.shrine.IncreasePoints(1000000, transform.position);
        }
    }
}
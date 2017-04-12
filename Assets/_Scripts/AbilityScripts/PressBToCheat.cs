// Author(s): Paul Calande
// Press B to gain tons of shrine points.
// Disable this script component when it's not being used for testing purposing.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressBToCheat : MonoBehaviour
{
    // Component references.
    private AbilityInterface cai;

    private void Awake()
    {
        cai = GetComponent<AbilityInterface>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            cai.shrine.points += 1000000;
        }
    }
}
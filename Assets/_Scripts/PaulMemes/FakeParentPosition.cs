// Author(s): Paul Calande
// Class that simulates Unity's parenting... but only for the GameObject's position.
// Rotation and scale are not included.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeParentPosition : MonoBehaviour
{
    // The "parent" transform.
    public Transform parent;

    private void Update()
    {
        if (parent != null)
        {
            transform.position = parent.position;
        }
    }
}
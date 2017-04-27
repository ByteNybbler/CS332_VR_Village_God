// Author(s): Paul Calande
// The Start method moves the GameObject to the position of the transform.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionOnStart : MonoBehaviour
{
    [Tooltip("The transform to move to.")]
    public Transform target;

    private void Start()
    {
        transform.position = target.position;
    }
}
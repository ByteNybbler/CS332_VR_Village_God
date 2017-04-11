// Author(s): Paul Calande
// This script will destroy the GameObject that it is attached to as soon as Update is called.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImmediately : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject);
    }
}
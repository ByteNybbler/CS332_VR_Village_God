// Author(s): Hunter Golden, Paul Calande
// Raycast script for VR Mountain God.

// Comment out the following line to prevent CastRay.cs debug messages.
//#define CASTRAY_DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastRay : MonoBehaviour
{
    // The layer masks for the raycast.
    // Environment is layer 8.
    private int layerMasks = 1 << 8;

    // Cast a ray. Returns true if the raycast hits.
    // Also takes a hit location as an out parameter.
    public bool Cast(out Vector3 location)
    {
#if CASTRAY_DEBUG
        Debug.DrawRay(transform.position, transform.forward, Color.green);
#endif
        RaycastHit rayCastData;
        if (Physics.Raycast(transform.position, transform.forward, out rayCastData, Mathf.Infinity, layerMasks))
        {
#if CASTRAY_DEBUG
            Debug.Log("CastRay: Hit at point: " + rayCastData.point);
#endif
            location = rayCastData.point;
            return true;
        }
        else
        {
            location = Vector3.zero;
            return false;
        }
	}
}
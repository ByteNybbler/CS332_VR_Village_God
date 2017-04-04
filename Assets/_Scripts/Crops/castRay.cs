// Author(s): Hunter Golden, Paul Calande
// Raycast script for VR Mountain God.

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
		Debug.DrawRay (transform.position, transform.forward, Color.green);
        RaycastHit RayCastData;
        if (Physics.Raycast(transform.position, transform.forward, out RayCastData, Mathf.Infinity, layerMasks))
        {
            Debug.Log("Hit At point: " + RayCastData.point);
            location = RayCastData.point;
            return true;
        }
        else
        {
            location = Vector3.zero;
            return false;
        }
	}
}	
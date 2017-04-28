// Author(s): Hunter Golden, Paul Calande
// Controller raycast script for VR Mountain God.

// Comment out the following line to prevent CastRay debugging.
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
    public bool Cast(out Vector3 location, float maxDistance)
    {
#if CASTRAY_DEBUG
        Debug.DrawRay(transform.position, transform.forward, Color.green);
#endif
        RaycastHit hit;
        bool raycastDidHit = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMasks);
        if (raycastDidHit)
        {
            location = hit.point;
        }
        else
        {
            location = transform.position + transform.forward * maxDistance;
        }
        return raycastDidHit;
	}

#if CASTRAY_DEBUG
    private void Update()
    {
        Vector3 location;
        Cast(out location, 100f);
        Debug.Log("CastRay debug Update hit location: " + location);
    }
#endif
}
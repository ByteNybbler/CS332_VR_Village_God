// Author(s): Paul Calande
// The Start method billboards the GameObject.
// It can be customized to only billboard on certain axes.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardOnStart : MonoBehaviour
{
    [Tooltip("Whether the object billboards on the x axis.")]
    public bool billboardX = true;
    [Tooltip("Whether the object billboards on the y axis.")]
    public bool billboardY = true;
    [Tooltip("Whether the object billboards on the z axis.")]
    public bool billboardZ = true;

    private void Start()
    {
        Vector3 oldEuler = transform.rotation.eulerAngles;
        Quaternion qCamera = Camera.main.transform.rotation;
        Vector3 newEuler = qCamera.eulerAngles;
        if (!billboardX)
        {
            newEuler.x = oldEuler.x;
        }
        if (!billboardY)
        {
            newEuler.y = oldEuler.y;
        }
        if (!billboardZ)
        {
            newEuler.z = oldEuler.z;
        }
        transform.rotation = Quaternion.Euler(newEuler);
    }
}
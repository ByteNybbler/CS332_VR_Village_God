// Author(s): Paul Calande
// Script that uses a referenced GameObject's children's transforms to create a list of transforms.
// These transforms can be used as a list of spawn positions, waypoints, etc.
// To make use of this functionality, follow these steps:
// 1. Create an empty GameObject in the scene.
// 2. Add empty GameObjects as children of that object.
// 3. Reference the parent object via Inspector in this script component's pointsContainer field.
// The child objects' transforms are all added to a list.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPoints : MonoBehaviour
{
    [Tooltip("Reference to the GameObject that has all of the positions as children.")]
    public GameObject pointsContainer;
    [Tooltip("The color of the sphere Gizmos in the scene view.")]
    public Color editorGizmoColor = Color.red;
    [Tooltip("The radius of the sphere Gizmos in the scene view.")]
    public float editorGizmoRadius = 0.2f;

    // List of the key points.
    private List<Transform> keyPoints = new List<Transform>();
    // Whether the list of key points has been populated yet.
    private bool hasCalculatedKeyPoints = false;

    // Helpful editor visuals.
    private void OnDrawGizmos()
    {
        if (pointsContainer != null)
        {
            // Draw a Gizmo at each key point, marking the transform.
            Transform[] allPoints = pointsContainer.GetComponentsInChildren<Transform>();
            foreach (Transform trans in allPoints)
            {
                if (trans.gameObject != pointsContainer)
                {
                    Gizmos.color = new Color(editorGizmoColor.r, editorGizmoColor.g, editorGizmoColor.b, 0.2f);
                    Gizmos.DrawSphere(trans.position, editorGizmoRadius);
                    Gizmos.color = editorGizmoColor;
                    Gizmos.DrawWireSphere(trans.position, editorGizmoRadius);
                }
            }
        }
    }

    // Calculate the list of transforms.
    private void CalculateKeyPoints()
    {
        Transform[] allPoints = pointsContainer.GetComponentsInChildren<Transform>();
        foreach (Transform trans in allPoints)
        {
            if (trans.gameObject != pointsContainer)
            {
                keyPoints.Add(trans);
            }
        }
        hasCalculatedKeyPoints = true;
    }

    // Get the list of transforms.
    public List<Transform> GetKeyPoints()
    {
        if (!hasCalculatedKeyPoints)
        {
            CalculateKeyPoints();
        }
        return keyPoints;
    }
}
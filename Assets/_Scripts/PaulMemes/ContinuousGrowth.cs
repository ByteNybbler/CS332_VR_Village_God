// Author(s): Paul Calande
// Script that causes objects to shrink until their scale hits a certain quantity.
// When the scale hits this quantity, the object is destroyed.
// Intended for objects where the x, y, and z scales are all always identical.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousGrowth : MonoBehaviour
{
    [Tooltip("The rate at which the GameObject grows. Use a negative value for shrinking.")]
    public float growthRate;
    [Tooltip("The scale at which the GameObject will be destroyed.")]
    public float targetScale;

    // The total amount of scale change so far.
    private float scaleDelta;
    // The target amount of scale change.
    private float targetDelta;

    private void Awake()
    {
        targetDelta = Mathf.Abs(transform.localScale.x - targetScale);
    }

    private void Update()
    {
        float scale = transform.localScale.x;
        float delta = growthRate * Time.deltaTime;
        scale += delta;
        scaleDelta += Mathf.Abs(delta);
        if (scaleDelta < targetDelta)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
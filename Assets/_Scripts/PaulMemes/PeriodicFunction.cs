// Author(s): Paul Calande
// Script that calls some function periodically.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PeriodicFunction : MonoBehaviour
{
    [Tooltip("The amount of time between each function call.")]
    public float period = 2.0f;
    [Tooltip("Function(s) to call.")]
    public UnityEvent function;

    private void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            function.Invoke();
            yield return new WaitForSeconds(period);
        }
    }
}
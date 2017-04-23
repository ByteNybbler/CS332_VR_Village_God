// Author(s): Paul Calande
// Script that calls some function periodically.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PeriodicFunction : MonoBehaviour
{
    [Tooltip("The amount of time before the first function call.")]
    public float initialDelay = 0f;
    [Tooltip("The amount of time between each function call.")]
    public float period = 2f;
    [Tooltip("Function(s) to call.")]
    public UnityEvent function;

    private void Start()
    {
        StartCoroutine(InitialDelay());
    }

    IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(initialDelay);
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
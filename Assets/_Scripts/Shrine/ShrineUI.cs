// Author(s): Paul Calande
// Shrine UI script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrineUI : MonoBehaviour
{
    [Tooltip("Reference to the shrine.")]
    public Shrine shrine;
    [Tooltip("Reference to the text.")]
    public Text text;

    private void Start()
    {
        shrine.PointsUpdated += Shrine_PointsUpdated;
    }

    private void OnDestroy()
    {
        shrine.PointsUpdated -= Shrine_PointsUpdated;
    }

    private void Shrine_PointsUpdated(int amount)
    {
        text.text = "Faith Points: " + amount.ToString();
    }
}
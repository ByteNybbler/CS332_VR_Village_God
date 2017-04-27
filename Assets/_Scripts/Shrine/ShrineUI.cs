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
    [Tooltip("Reference to the wave controller.")]
    public WaveController waveController;
    [Tooltip("Reference to the text keeping track of the shrine points.")]
    public Text textPoints;
    [Tooltip("Reference to the text keeping track of the current wave.")]
    public Text textWave;

    private void Start()
    {
        shrine.PointsUpdated += Shrine_PointsUpdated;
        waveController.WaveStarted += WaveController_WaveStarted;
    }

    private void OnDestroy()
    {
        shrine.PointsUpdated -= Shrine_PointsUpdated;
        waveController.WaveStarted -= WaveController_WaveStarted;
    }

    private void Shrine_PointsUpdated(int amount)
    {
        textPoints.text = "Faith Points: " + amount;
    }

    private void WaveController_WaveStarted(int number)
    {
        if (number == 0)
        {
            textWave.text = "";
        }
        else
        {
            textWave.text = "Current Wave: " + number;
        }
    }
}
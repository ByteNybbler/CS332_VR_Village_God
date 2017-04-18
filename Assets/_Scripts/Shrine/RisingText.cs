// Author(s): Paul Calande
// Rising text script. To be attached to a worldspace canvas with a text object as a child.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RisingText : MonoBehaviour
{
    // The speed at which the canvas is rising.
    // The value in the Inspector determines the initial rising speed.
    public float risingSpeed;
    // The rate at which the rising speed decays.
    public float risingSpeedDecayRate;
    // How many seconds the text takes to start decaying.
    public float alphaStartingSeconds;
    // How quickly the alpha decreases.
    public float alphaDecayRate;
    // Reference to the text component.
    public Text text;

    // The alpha value of the text.
    private float alpha = 1f;
    // The number of seconds passed since the text was instantiated.
    private float timePassed = 0f;

    private void Update()
    {
        float deltaTimeFixed = Time.deltaTime / Time.timeScale;
        // Calculate the canvas' new position.
        Vector3 newpos = transform.position;
        newpos.y += risingSpeed * deltaTimeFixed;
        // Decrease the rising speed.
        risingSpeed -= risingSpeedDecayRate * deltaTimeFixed;
        // Update the position.
        transform.position = newpos;
        // Cap the rising speed with a minimum of 0.
        if (risingSpeed < 0f)
        {
            risingSpeed = 0f;
        }
        // Update the text color.
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        // If enough time has passed...
        if (timePassed > alphaStartingSeconds)
        {
            // Decay the alpha.
            alpha -= alphaDecayRate * deltaTimeFixed;
            // Destroy the object when there's no alpha left.
            if (alpha < 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            timePassed += deltaTimeFixed;
        }
    }

    public void SetTextString(string str)
    {
        text.text = str;
    }

    public void SetTextColor(Color col)
    {
        text.color = col;
    }

    public void SetTextSize(int size)
    {
        text.fontSize = size;
    }
}
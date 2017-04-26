// Author(s): Paul Calande
// Class for changing an image's alpha over time.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public enum State
    {
        Neutral, // Do nothing.
        FadeIn, // Fade in.
        FadeOut // Fade out.
    }

    [Tooltip("Reference to the image to fade.")]
    public Image image;
    [Tooltip("The current state of the fade.")]
    public State state = State.Neutral;
    [Tooltip("The maximum alpha value.")]
    public float alphaMax = 1f;
    [Tooltip("The minimum alpha value.")]
    public float alphaMin = 0f;
    [Tooltip("The current alpha value.")]
    public float alphaCurrent = 0f;
    [Tooltip("How quickly the image fades in. 1.0 is full opacity.")]
    public float fadeInSpeed;
    [Tooltip("How quickly the image fades out. 1.0 is full opacity.")]
    public float fadeOutSpeed;

    // Invoked when alphaCurrent reaches alphaMax.
    public delegate void AlphaHitMaxHandler();
    public event AlphaHitMaxHandler AlphaHitMax;
    // Invoked when alphaCurrent reaches alphaMin.
    public delegate void AlphaHitMinHandler();
    public event AlphaHitMinHandler AlphaHitMin;

    private void Start()
    {
        UpdateImage();
    }

    private void Update()
    {
        switch (state)
        {
            case State.FadeIn:
                IncrementAlpha();
                break;

            case State.FadeOut:
                DecrementAlpha();
                break;
        }
    }

    private void IncrementAlpha()
    {
        alphaCurrent += fadeInSpeed * Time.deltaTime;
        if (alphaCurrent >= alphaMax)
        {
            alphaCurrent = alphaMax;
            state = State.Neutral;
            OnAlphaHitMax();
        }
        UpdateImage();
    }

    private void DecrementAlpha()
    {
        alphaCurrent -= fadeOutSpeed * Time.deltaTime;
        if (alphaCurrent <= alphaMin)
        {
            alphaCurrent = alphaMin;
            state = State.Neutral;
            OnAlphaHitMin();
        }
        UpdateImage();
    }

    private void UpdateImage()
    {
        // Calculate the new color based on the current alpha value.
        Color fadeColor = new Color(1f, 1f, 1f, alphaCurrent);
        // Update the image color.
        image.color = fadeColor;
    }

    // Event invocations.
    private void OnAlphaHitMax()
    {
        if (AlphaHitMax != null)
        {
            AlphaHitMax();
        }
    }
    private void OnAlphaHitMin()
    {
        if (AlphaHitMin != null)
        {
            AlphaHitMin();
        }
    }
}
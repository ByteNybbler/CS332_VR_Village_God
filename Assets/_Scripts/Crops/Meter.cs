// Author(s): Hunter Golden, Paul Calande
// General purpose meter script (for health, build progress, etc).
// Attach this script to any GameObject that should have a meter as a child.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    [Tooltip("The meter prefab.")]
    public GameObject prefabMeter;
    [Tooltip("The color of the meter that indicates the meter's progress; the front color.")]
    public Color colorFront = Color.green;
    [Tooltip("The color of the meter that isn't covered by the progress; the back color.")]
    public Color colorBack = Color.red;
    [Tooltip("The color of the meter's border.")]
    public Color colorBorder = Color.black;
    [Tooltip("The position of the meter relative to this component's gameObject.")]
    public Vector3 relativePosition;
    [Tooltip("The total width of the meter (excluding border).")]
    public float width;
    [Tooltip("The total height of the meter (excluding border).")]
    public float height;
    [Tooltip("How thick the border is. This quantity is added to the base width and height of the meter.")]
    public float borderThickness;
    [Tooltip("If true, the meter will become invisible when it's full.")]
    public bool hideMeterOnFull = false;
    [Tooltip("If true, the meter will become invisible when it's empty.")]
    public bool hideMeterOnEmpty = false;

    // Current percentage of the meter that is filled.
    private float currentPercentage;
    // The current quantity stored in the meter. This determines how much of the meter is filled.
    private float currentValue;
    // Minimum percentage of the meter.
    private float minPercentage = 0f;
    // Maximum percentage of the meter.
    private float maxPercentage = 1f;
    // How many percentage units there are per unit value.
    private float percentPerUnitValue;
    // The meter parent object that contains all of the pieces of the meter.
    private GameObject meter;
    // The pieces of the progress meter.
    private GameObject meterFront;
    private GameObject meterBack;
    private GameObject meterBorder;

    private void Awake()
    {
        meter = Instantiate(prefabMeter, transform.position + relativePosition, Quaternion.identity);
        // Set the meter's parent to this GameObject.
        meter.transform.SetParent(transform, true);
        // Get the meter's children.
        meterBorder = meter.transform.GetChild(0).gameObject;
        meterBack = meter.transform.GetChild(1).gameObject;
        meterFront = meter.transform.GetChild(2).gameObject;
        // Get the dimensions of the canvas to properly position the front meter.
        //Vector2 canvasDimensions = meter.GetComponent<RectTransform>().sizeDelta;
        //Debug.Log("Meter: canvas x: " + canvasDimensions.x + ", canvas y: " + canvasDimensions.y);

        // Set the properties of the meter pieces appropriately.
        // The anchor of the front piece of the meter is different, so its x offset must be set appropriately.
        // (canvasDimensions.x - width) * 0.5f
        SetPieceProperties(meterFront, width, height, colorFront, -width * 0.5f);
        SetPieceProperties(meterBack, width, height, colorBack, 0f);
        SetPieceProperties(meterBorder, width + borderThickness, height + borderThickness, colorBorder, 0f);
    }

    /// <summary>
    /// Set the properties of one of the meter pieces.
    /// </summary>
    /// <param name="piece">Which piece to modify.</param>
    /// <param name="pWidth">The new width of the piece.</param>
    /// <param name="pHeight">The new height of the piece.</param>
    /// <param name="col">The new color of the piece.</param>
    /// <param name="xOffset">The x offset of the piece.</param>
    private void SetPieceProperties(GameObject piece, float pWidth, float pHeight, Color col, float xOffset)
    {
        RectTransform rt;
        Image img;
        rt = piece.GetComponent<RectTransform>();
        img = piece.GetComponent<Image>();
        rt.sizeDelta = new Vector2(pWidth, pHeight);
        img.color = col;
        Vector3 offset = new Vector3(xOffset, 0f, 0f);
        //Debug.Log("SetPieceProperties offset: " + offset);
        rt.localPosition = offset;
    }

    /// <summary>
    /// Sets both the current and maximum values of the meter.
    /// Mainly used to set the progress on the meter for the first time.
    /// This function is more efficient than calling SetCurrentValue and SetMaxValue separately.
    /// </summary>
    /// <param name="newCurrentValue">The new current value of the meter.</param>
    /// <param name="newMaxValue">The new maximum value of the meter.</param>
    public void SetBothValues(float newCurrentValue, float newMaxValue)
    {
        currentValue = newCurrentValue;
        SetMaxValue(newMaxValue);
    }

    /// <summary>
    /// Set a new max value and calculate the new percent per unit value.
    /// </summary>
    /// <param name="maxValue">The new max value.</param>
    public void SetMaxValue(float maxValue)
    {
        percentPerUnitValue = maxPercentage / maxValue;
        // Now that we have a new percent per unit value, update the progress of the meter.
        SetCurrentValue(currentValue);
    }

    /// <summary>
    /// Set the current progress value of the meter.
    /// </summary>
    /// <param name="value">The new value.</param>
    public void SetCurrentValue(float value)
    {
        currentValue = value;
        currentPercentage = value * percentPerUnitValue;
        meterFront.transform.localScale = new Vector3(currentPercentage, 1f, 1f);

        // Enable or disable the meter depending on the progress of the meter.
        if (currentPercentage < maxPercentage)
        {
            // The meter is less than the maximum.
            if (currentPercentage > minPercentage)
            {
                // The meter is not at the minimum or the maximum.
                // It should be enabled no matter what settings we have.
                meter.SetActive(true);
            }
            else
            {
                // The meter is at the minimum.
                if (hideMeterOnEmpty)
                {
                    meter.SetActive(false);
                }
            }
        }
        else
        {
            // The meter is at the maximum.
            if (hideMeterOnFull)
            {
                meter.SetActive(false);
            }
        }
    }
}
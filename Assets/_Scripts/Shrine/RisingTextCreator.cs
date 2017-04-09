// Author(s): Paul Calande
// Class for creating a rising text prefab.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingTextCreator : MonoBehaviour
{
    [Tooltip("Prefab for the rising text UI canvas.")]
    public GameObject prefabRisingText;
    [Tooltip("The string to display.")]
    public string message;
    [Tooltip("The color of the rising text.")]
    public Color textColor;
    [Tooltip("The offset of the rising text's spawning position.")]
    public Vector3 spawnOffset;

    public void CreateRisingText(Vector3 position)
    {
        // Instantiate the +1 canvas.
		//Debug.Log(name + " is trying to create rising text!");
        GameObject plusOne = Instantiate(prefabRisingText, position + spawnOffset, Quaternion.identity);
        RisingText rt = plusOne.GetComponent<RisingText>();
        rt.SetTextString(message);
        rt.SetTextColor(textColor);
    }
}
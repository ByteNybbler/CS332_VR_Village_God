// Author(s): Hunter Golden, Paul Calande
// Crop growing script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour
{
	[Tooltip("The speed at which the plants grow.")]
	public float rate;
	[Tooltip("The maximum size of the plant.")]
	public float maxSize;
	public float scale;
	[Tooltip("Whether the plant is fully grown.")]
	public bool isGrown;

	void Awake()
    {
		scale = 0f;
		isGrown = false;
	}

	void Update()
    {
		if (scale < maxSize)
        {
            float increase = rate * Time.deltaTime;
			gameObject.transform.localScale += Vector3.up * increase;
			gameObject.transform.position += (Vector3.up * increase)/2;
			scale += increase;
		}
        else
        {
			isGrown = true;
		}
	}
}
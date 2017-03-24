using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour {
	//how fast the plants will grow
	public float rate;
	//maximum size of the plant
	public float maxSize;
	public float scale;
	//checks if the plant is fully grown
	public bool isGrown;

	// Use this for initialization
	void Start () {
		scale = 0;
		isGrown = false;
	}
	
	// Update is called once per frame
	void Update () {
				
		if (scale < maxSize) {
			gameObject.transform.localScale += Vector3.up * rate;
			gameObject.transform.position += (Vector3.up * rate)/2;
			scale += rate * Time.deltaTime;
		} else {
			isGrown = true;
		}
	}
}

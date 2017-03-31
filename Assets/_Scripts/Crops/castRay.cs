using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castRay : MonoBehaviour {
	private RaycastHit RayCastData;
	private int layerMasks;
	public Vector3 location;

	void start (){

	}

	void Update (){
		Debug.DrawRay (transform.position, transform.forward, Color.green);
		if (Physics.Raycast (transform.position, transform.forward, out RayCastData, Mathf.Infinity, layerMasks)) {
			Debug.Log ("Hit At point: " + RayCastData.point);
			location = RayCastData.point;
		}
	}
	private void Awake()
	{
		// Environment is layer 8.
		layerMasks = 1 << 8;
	}


}	
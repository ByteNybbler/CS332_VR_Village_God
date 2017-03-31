using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castRay : MonoBehaviour {

	private int distance = 100;
	private RaycastHit RayCastData;
	public GameObject crop;
	private int layerMasks;

	private void Awake()
	{
		// Environment is layer 8.
		layerMasks = 1 << 8;
	}

	public void makeFood ()
	{

		Debug.DrawRay (transform.position, transform.forward, Color.green);
		 	
		if (Physics.Raycast (transform.position, transform.forward, out RayCastData, Mathf.Infinity, layerMasks)) {
			Debug.Log ("Hit At point: " + RayCastData.point);
			Instantiate(crop, RayCastData.point, Quaternion.identity);
		}

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castRay : MonoBehaviour {
	public GameObject shrine;
	private Shrine shrineComponent;
	private int distance = 100;
	private RaycastHit RayCastData;
	public GameObject crop;
	private int layerMasks;
	private int cropCount;
	public Vector3 location;

	void start (){
		shrineComponent = shrine.GetComponent<Shrine> ();
		cropCount = 0;
	}
	private void Awake()
	{
		// Environment is layer 8.
		layerMasks = 1 << 8;
	}

	public void MakeFood ()
	{
	
		Debug.DrawRay (transform.position, transform.forward, Color.green);
		 	              
		if ((shrineComponent.SpendPoints (10)) && (cropCount < 11)) {
			if (Physics.Raycast (transform.position, transform.forward, out RayCastData, Mathf.Infinity, layerMasks)) {
				Debug.Log ("Hit At point: " + RayCastData.point);
				location = RayCastData.point;
				GameObject cropInstance = Instantiate (crop, RayCastData.point, Quaternion.identity);
				cropInstance.GetComponent<plantHealth> ().OnDeath += OnCropDeath;
				cropCount++;
			}
		}
	}

	public void OnCropDeath()
	{
		cropCount--;
	}
}	
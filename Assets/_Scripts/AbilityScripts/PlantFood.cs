using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantFood : MonoBehaviour
{
	private int cropCount;
	public GameObject crop;
	public GameObject shrine;
	private Shrine shrineComponent;
	public Vector3 location;
	public GameObject locationObj;

	void start (){
		//shrineComponent = shrine.GetComponent<Shrine> ();
		cropCount = 0;
	}

	public void MakeFood ()
	{
		shrineComponent = shrine.GetComponent<Shrine> ();
		location = locationObj.GetComponent<castRay> ().location;
		if (cropCount < 10) {
			if (shrineComponent.SpendPoints (10)) {
				GameObject cropInstance = Instantiate (crop, location, Quaternion.identity);
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

	/*
	private void Update()
	{
		TryToPlaceFood ();
	}
	*/



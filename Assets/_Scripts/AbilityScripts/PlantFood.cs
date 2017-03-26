using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlantFood : VRTK_SimplePointer
{
	[Tooltip("The food prefab.")]
	public GameObject foodPrefab;

	public void TryToPlaceFood()
	{
		// Attempt to raycast.
		Ray pointerRaycast = new Ray(GetOriginPosition(), GetOriginForward());
		RaycastHit hit;
		if (Physics.Raycast (pointerRaycast, out hit, pointerLength, ~layersToIgnore))
		{
			// Place the food at the hit.
			Vector3 foodPosition = hit.transform.position;
			Debug.Log ("Placed food at " + foodPosition);
			Instantiate (foodPrefab, foodPosition, Quaternion.identity);
		}
	}

	/*
	private void Update()
	{
		TryToPlaceFood ();
	}
	*/
}

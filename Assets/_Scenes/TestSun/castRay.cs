using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castRay : MonoBehaviour {


	void Update (){
		RaycastHit hit;
		float theDistance;

		Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;

		Debug.DrawRay (transform.position, forward, Color.green);
		transform.Rotate(1,0,0);
		if(Physics.Raycast(transform.position, (forward), out hit)){
			theDistance = hit.distance;
			//print (theDistance + " " + hit.collider.gameObject.name);
			Debug.Log(hit.transform.position);
	}
}

}
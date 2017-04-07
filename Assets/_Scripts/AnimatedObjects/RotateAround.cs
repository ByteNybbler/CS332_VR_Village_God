using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	//Script that rotates a gameObject around its origin

	void Update () {
		gameObject.transform.RotateAround (gameObject.transform.position, Vector3.right, 20 * Time.deltaTime);
	}
}

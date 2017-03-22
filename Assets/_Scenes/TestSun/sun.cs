using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun : MonoBehaviour {

	public Transform center;
	public float speed;

	void Update () {
				gameObject.transform.RotateAround(center.transform.position, Vector3.left, speed * Time.deltaTime);
	}
}

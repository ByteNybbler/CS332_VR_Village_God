using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {
	[Tooltip("The GameObject to be destroyed.")]
	public GameObject objectToDestroy;

	[Tooltip("The amount of time before the Object is destroyed")]
	public int time;
	// Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (objectToDestroy, time);
	}
}

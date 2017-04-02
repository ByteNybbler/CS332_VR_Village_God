using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public Text totalPoints;
	public GameObject shrine;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		totalPoints.text = "Faith Points: " + shrine.GetComponent<Shrine>().points.ToString();
	}
}

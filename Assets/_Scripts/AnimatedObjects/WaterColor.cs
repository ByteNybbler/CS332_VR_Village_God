using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColor : MonoBehaviour {


	void Start () {
		this.GetComponent<ParticleSystem> ().startColor = new Color (1, 0, 1, .5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

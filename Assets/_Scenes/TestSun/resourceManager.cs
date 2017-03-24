using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceManager : MonoBehaviour {

	public int Faith;
	public int Wood;
	public int Food;
	Shrine shrineScript;
	void Start () {
		shrineScript = GameObject.Find ("Shrine").GetComponent<Shrine> ();
		Faith = 0;
		Wood = 0;
		Food = 0;
	}

	void Update () {
		Faith = shrineScript.points;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public int faith;
	public int wood;
	public int food;

    public GameObject shrineInstance;

	private Shrine shrineScript;

	void Start ()
    {
		shrineScript = shrineInstance.GetComponent<Shrine> ();
		faith = 0;
		wood = 0;
		food = 0;
	}

	void Update ()
    {
		faith = shrineScript.points;
	}
}

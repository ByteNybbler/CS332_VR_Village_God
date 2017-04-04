using UnityEngine;
using System.Collections;

public class FunnyScript : MonoBehaviour {
	public float pokeForce;

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit))
				Debug.Log(hit.transform.position);
		}
	}
}
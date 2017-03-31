using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantHealth : MonoBehaviour {

	public delegate void DeathAction();
	public event DeathAction OnDeath;

	private int health;

	void Start () {
		health = 10;
	}

	public void DecreaseHealth(){
		health--;
		if (health == 10) {
			
			if (OnDeath != null) {
				// Call the death event.
				OnDeath ();
			}

			Destroy (gameObject);

		}
	}
}

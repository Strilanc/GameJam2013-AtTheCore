using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
	public AudioSource clack;
	
	void OnCollisionEnter(Collision other){
		clack.Play();


	}
}

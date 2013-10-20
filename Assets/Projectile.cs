using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	void Update () {
		transform.Translate(transform.forward *1000*Time.deltaTime);
	}
}

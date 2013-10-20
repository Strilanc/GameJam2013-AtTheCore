using UnityEngine;
using System.Collections;

public class SpinStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    rigidbody.angularVelocity += new Vector3(7, 0, 0);
	    rigidbody.inertiaTensor *= 500;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

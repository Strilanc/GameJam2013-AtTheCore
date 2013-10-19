using UnityEngine;
using System.Collections;

public class Gymbal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Inverse(transform.parent.rotation);
	}
}

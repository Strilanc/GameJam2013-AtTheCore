using UnityEngine;
using System.Collections;

public class SuperSphere : MonoBehaviour {
	void Start () {
	}
	
	void Update () {
	
	}

    void OnCollisionEnter(Collision other) {
        Application.LoadLevel("end");
    }
}

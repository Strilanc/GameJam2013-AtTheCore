using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
    public double RotationSpeed;
	void Start () {
    }
	
	void Update () {
        var yaw = Input.GetAxis("Horizontal") * Time.deltaTime * 1f;
        var pitch = Input.GetAxis("Vertical") * Time.deltaTime * 1f;
        var roll = Input.GetAxis("Roll") * 0.01f;

	    var thrust = Input.GetKey(KeyCode.Joystick1Button0) ? 1 : 0;
	    var rigidBody = GetComponent<Rigidbody>();
	    var a = rigidBody.angularVelocity;

        var p1 = transform.right;
        var p2 = transform.up;
        var p3 = transform.forward;
        
        rigidBody.velocity += transform.forward.normalized*0.1f*thrust;
        rigidBody.angularVelocity += p2 * yaw;
        rigidBody.angularVelocity += p1 * pitch;
        rigidBody.angularVelocity += p3 * roll;
	}
}

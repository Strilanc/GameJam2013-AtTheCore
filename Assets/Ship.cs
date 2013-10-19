using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
    public GameObject SpeedStreak;
    private readonly List<GameObject> _speedStreak = new List<GameObject>(); 
	void Start () {
    }
	
	void FixedUpdate () {
	    const float rotFactor = 0.5f;
        var yaw = Input.GetAxis("Horizontal") * Time.deltaTime * rotFactor;
        var pitch = Input.GetAxis("Vertical") * Time.deltaTime * rotFactor;
        var roll = Input.GetAxis("Roll") * Time.deltaTime * rotFactor;

	    var rigidBody = GetComponent<Rigidbody>();
        rigidBody.angularVelocity += transform.up * yaw;
        rigidBody.angularVelocity += transform.right * pitch;
        rigidBody.angularVelocity += transform.forward * roll;

        var thrust = (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Space)) ? 1 : 0;
        rigidBody.velocity += transform.forward.normalized * 0.1f * thrust;
        if (thrust != 0) {
            // rattle ship
            var v = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))/500;
            transform.rotation *= Quaternion.FromToRotation(Vector3.forward, Vector3.forward + v);
        }

	    MakeSpeedStreak();

	    // rotation compensation
	    /* var rotationSpeed = rigidBody.angularVelocity.magnitude;
	    var autoBrakingSpeed = Mathf.Max(rotationSpeed - 0.1f, 0)/5*Time.deltaTime;
	    rigidBody.angularVelocity -= rigidBody.angularVelocity.normalized*autoBrakingSpeed;*/
	}
    void MakeSpeedStreak() {
        foreach (var s in _speedStreak.ToArray()) {
            if ((s.transform.position - transform.position).sqrMagnitude > 100 * 100) {
                DestroyObject(s.gameObject);
                _speedStreak.Remove(s);
            } else if (s.GetComponent<SpeedStreak>().life > 30) {
                DestroyObject(s.gameObject);
                _speedStreak.Remove(s);
            }
        }

        while (_speedStreak.Count < 500) {
            var r = transform.position + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 100;
            var g = (GameObject)Instantiate(SpeedStreak, r, Quaternion.identity);
            g.rigidbody.velocity = -g.transform.position.normalized*50;
            _speedStreak.Add(g);
        }
    }
}

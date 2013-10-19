using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
    public GameObject SpeedStreak;
    private readonly List<GameObject> _speedStreak = new List<GameObject>(); 
	void Start () {
    }
	
	void FixedUpdate () {
        const float rotFactor = 2f;
        const float sideThrustFactor = 50f;
        var yaw = Input.GetAxis("Horizontal") * Time.deltaTime * rotFactor;
        var pitch = Input.GetAxis("Vertical") * Time.deltaTime * rotFactor;
        var roll = Input.GetAxis("Roll") * Time.deltaTime * rotFactor;

        var sideToSide = Input.GetAxis("LeftRightThrust") * Time.deltaTime * sideThrustFactor;
        var upDOwn = Input.GetAxis("UpDownThrust") * Time.deltaTime * sideThrustFactor;

	    var rigidBody = GetComponent<Rigidbody>();
        rigidBody.angularVelocity += transform.up * yaw;
        rigidBody.angularVelocity += transform.right * pitch;
        rigidBody.angularVelocity += transform.forward * roll;

        rigidBody.velocity += sideToSide * -transform.right;
        rigidBody.velocity += upDOwn * transform.up;

        var thrust = Input.GetKey(KeyCode.Joystick1Button0) ? 1 : 0;
        rigidBody.velocity += transform.forward.normalized * 1f * thrust;
        if (thrust != 0) {
            // rattle ship
            var v = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))/500;
            transform.rotation *= Quaternion.FromToRotation(Vector3.forward, Vector3.forward + v);
        }

	    MakeSpeedStreak();

	    var useBrakes = Input.GetKey(KeyCode.Joystick1Button1);
	    var brakesPlus = useBrakes ? 100f : 0;

	    // rotation compensation
	    var rotationSpeed = rigidBody.angularVelocity.magnitude;
        var autoBrakingSpeed = Mathf.Clamp((rotationSpeed - 0.1f) / 3 * Time.deltaTime, 0, rotationSpeed);
	    rigidBody.angularVelocity -= rigidBody.angularVelocity.normalized*autoBrakingSpeed;

	    var glide = rigidBody.velocity - RestAtPoint(transform.position);
        var autoBrakingSpeed2 = Mathf.Clamp((glide.magnitude - 0.1f) / 3 * Time.deltaTime, 0, glide.magnitude);
        rigidBody.velocity -= glide.normalized * autoBrakingSpeed2;
	}
    private static Vector3 RestAtPoint(Vector3 position) {
        return -position.normalized*25;
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
            g.rigidbody.velocity = RestAtPoint(g.transform.position);
            _speedStreak.Add(g);
        }
    }
}

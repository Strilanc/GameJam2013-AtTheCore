using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour {
    public GameObject Head;
    public ParticleSystem EngineLeftDownward;
    public ParticleSystem EngineLeftUpward;
    public ParticleSystem EngineLeftLeftward;
    public ParticleSystem EngineLeftForeward;
    public ParticleSystem EngineLeftBackward;

    public ParticleSystem EngineRightDownward;
    public ParticleSystem EngineRightRightward;
    public ParticleSystem EngineRightUpward;
    public ParticleSystem EngineRightForeward;
    public ParticleSystem EngineRightBackward;

    public ParticleSystem EngineBottomForeward;
    public ParticleSystem EngineBottomBackward;

    public List<Material> AsteroidMaterials; 
	
	public AudioSource audioSource;
	public AudioSource engine;
	public GameObject normalCam;
    public GameObject occulusCam;
    public GameObject occulusCam_Right;
    public GameObject occulusCam_Left;
	
	public Animator stick;
	public float pitch;
	public float yaw;
    private float gunCooldown = 0;
	
	public Gun gun;
	
	void Start () {
		
	    this.rigidbody.inertiaTensor *= 5;
		if(!OVRDevice.IsSensorPresent(0)){
			occulusCam.SetActive(false);
			normalCam.SetActive(true);
		}
		
    }

    void FixedUpdate() {

        gunCooldown -= Time.deltaTime;
        if (gunCooldown <= 0 && gun != null && Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.Return)) {
			gun.shoot = true;
            gunCooldown += 0.1f;
        }else {
			gun.shoot = false;
		}
        gunCooldown = Mathf.Max(gunCooldown, 0);
		
        var yawInput = Input.GetAxis("Horizontal");
        var pitchInput = Input.GetAxis("Vertical");
        var rollInput = Input.GetAxis("Roll");
        
		stick.SetFloat("Pitch",pitchInput);
		stick.SetFloat("Yaw",yawInput);
		
        var angularThrust = default(Vector3);
        angularThrust += transform.up * yawInput;
        angularThrust += transform.right * pitchInput;
        angularThrust += transform.forward * rollInput;

        var inputLeftRightThrust = Input.GetAxis("LeftRightThrust");
        var inputUpDownThrust = Input.GetAxis("UpDownThrust");
        var inputForwardBackThrust = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0) ? 1 : 0f;
        var thrustVector = default(Vector3);
        thrustVector += transform.forward.normalized * inputForwardBackThrust;
        thrustVector += inputLeftRightThrust * -transform.right;
        thrustVector += inputUpDownThrust * transform.up;
		
		
        var rigidBody = GetComponent<Rigidbody>();

        if (inputForwardBackThrust != 0) {
            // rattle ship
            var v = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) / 500;
            transform.rotation *= Quaternion.FromToRotation(Vector3.forward, Vector3.forward + v);
        }


        MakeSpeedStreak();

        var useBrakes = Input.GetKey(KeyCode.Joystick1Button1);
        const float enginePower = 50f;
        const float rotationalPower = 2f;
        var brakingFactor = useBrakes ? 1f : 0.5f;
        var rotationalBrakingFactor = useBrakes ? 0.9f : 0.5f;

        // rotation compensation
        if (angularThrust.magnitude < 0.1 || useBrakes) {
            var glide = rigidBody.angularVelocity;
            var brakeThrustVector = -glide.normalized * rotationalBrakingFactor;
            var velocityAfterBraking = rigidBody.angularVelocity + brakeThrustVector * rotationalPower * Time.deltaTime;
            var overBraked = Mathf.Sign(Vector3.Dot(rigidBody.angularVelocity, brakeThrustVector)) != Mathf.Sign(Vector3.Dot(velocityAfterBraking, brakeThrustVector));
            if (overBraked) brakeThrustVector = default(Vector3);
            angularThrust += brakeThrustVector;
        }
        rigidBody.angularVelocity += angularThrust*rotationalPower*Time.deltaTime;

        if (thrustVector.magnitude < 0.1 || useBrakes) {
            var glide = rigidBody.velocity - RestAtPoint(transform.position);
            var brakeThrustVector = -glide.normalized * brakingFactor;
            var glideAfterBraking = glide + brakeThrustVector*enginePower*Time.deltaTime;
            var overBraked = Vector3.Dot(glide, brakeThrustVector) * Vector3.Dot(glideAfterBraking, brakeThrustVector) < 0;
            if (!overBraked) thrustVector += brakeThrustVector;
        }
        rigidBody.velocity += thrustVector * Time.deltaTime * enginePower;

        var outputThrustUpDown = Vector3.Dot(thrustVector, transform.up);
        var outputThrustLeftRight = Vector3.Dot(thrustVector, transform.right);
        var outputThrustForwardBack = Vector3.Dot(thrustVector, transform.forward);

        var outputAngularThrustUpDown = Vector3.Dot(angularThrust, transform.up);
        var outputAngularThrustLeftRight = Vector3.Dot(angularThrust, transform.right);
        var outputAngularThrustForwardBack = Vector3.Dot(angularThrust, transform.forward);

        const float emissionFactor = 50;

        Func<float, float> emissionRateForOutput = output => {
            if (output < 0) return 0;
            return Mathf.Clamp(output, 0, 1)*emissionFactor;
        };
        EngineLeftDownward.emissionRate = emissionRateForOutput(-outputAngularThrustForwardBack + outputThrustUpDown);
        EngineLeftUpward.emissionRate = emissionRateForOutput(outputAngularThrustForwardBack - outputThrustUpDown);
        EngineRightDownward.emissionRate = emissionRateForOutput(outputAngularThrustForwardBack + outputThrustUpDown);
        EngineRightUpward.emissionRate = emissionRateForOutput(-outputAngularThrustForwardBack - outputThrustUpDown);

        EngineBottomForeward.emissionRate = emissionRateForOutput(outputAngularThrustLeftRight);
        EngineBottomBackward.emissionRate = emissionRateForOutput(-outputAngularThrustLeftRight);

        EngineLeftLeftward.emissionRate = emissionRateForOutput(outputThrustLeftRight);
        EngineRightRightward.emissionRate = emissionRateForOutput(-outputThrustLeftRight);

        EngineLeftForeward.emissionRate = emissionRateForOutput(-outputAngularThrustUpDown - outputThrustForwardBack);
        EngineLeftBackward.emissionRate = emissionRateForOutput(outputAngularThrustUpDown + outputThrustForwardBack);
        EngineRightForeward.emissionRate = emissionRateForOutput(outputAngularThrustUpDown - outputThrustForwardBack);
        EngineRightBackward.emissionRate = emissionRateForOutput(-outputAngularThrustUpDown + outputThrustForwardBack);

        var t = transform.position - thrustVector / 10 + transform.up * 0.4f + transform.forward * 0.05f;
        var q = transform.rotation * Quaternion.AngleAxis(-angularThrust.magnitude * 10, angularThrust.normalized);
        var lambda = 1 - Mathf.Pow(0.1f, Time.deltaTime);
        Head.transform.position = Vector3.Lerp(Head.transform.position, t, lambda);
        Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, q, lambda);
		engine.pitch = thrustVector.magnitude+0.5f;

        var xc = Mathf.Clamp(Vector3.Dot(transform.forward, -transform.position.normalized), 0, 1);
        var zc = xc * xc / 100;
        xc *= xc * 2;
        var yc = Mathf.Clamp(Vector3.Dot(transform.forward, transform.position.normalized), 0, 1);
        yc *= yc / 4;
        normalCam.GetComponent<Bloom>().bloomIntensity = xc;
        occulusCam_Right.GetComponent<Bloom>().bloomIntensity = xc;
        occulusCam_Left.GetComponent<Bloom>().bloomIntensity = xc;
        //normalCam.GetComponent<NoiseEffect>().grainIntensityMax = yc;
        //occulusCam_Right.GetComponent<NoiseEffect>().grainIntensityMax = yc;
        //occulusCam_Left.GetComponent<NoiseEffect>().grainIntensityMax = yc;

        foreach (var m in AsteroidMaterials) {
            m.SetColor("_OutlineColor", new Color(255, 255, 255, Mathf.Clamp(zc*50, 0, 1)));
            m.SetFloat("_Outline", zc);
        }
    }
    private static Vector3 RestAtPoint(Vector3 position) {
        return default(Vector3);
    }
    void MakeSpeedStreak() {
        this.GetComponent<ParticleSystem>().transform.position = this.transform.position;
        this.GetComponent<ParticleSystem>().emissionRate = this.rigidbody.velocity.magnitude*25;
        this.GetComponent<ParticleSystem>().startLifetime = Mathf.Clamp(this.rigidbody.velocity.magnitude, 0.01f, 1);
    }
	
	public void OnCollisonEnter(Collision other){
		Debug.Log (other.collider.name);
		audioSource.Play();
	}
}

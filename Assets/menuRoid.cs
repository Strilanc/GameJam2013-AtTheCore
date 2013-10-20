using UnityEngine;
using System.Collections;

public class menuRoid : MonoBehaviour {
	public float minSpeed = -10.0f;
	public float maxSpeed = 10.0f;
	public float rotateSpeedMin = -25.0f;
	public float rotateSpeedMax = 25.0f;
	public float rand1;
	public float rand2;
	public float randomSpeed;
	public float minUp = -5.0f;
	public float maxUp = 5.0f;
	public float ups;
	public MenuSpawner spawner;
	// Use this for initialization
	void Start () {
		rand1 = Random.Range(rotateSpeedMin, rotateSpeedMax);
		rand2 = Random.Range(rotateSpeedMin, rotateSpeedMax);
		randomSpeed = Random.Range(minSpeed, maxSpeed);
		ups = Random.Range (minUp, maxUp);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(
			transform.position.x - 1 * Time.deltaTime * randomSpeed, 
			transform.position.y + 1 * Time.deltaTime * ups, 
			transform.position.z - 1 * Time.deltaTime * randomSpeed);
		
		transform.Rotate(Vector3.right * Time.deltaTime * rand1);
		transform.Rotate(Vector3.up * Time.deltaTime * rand2);
		
		if (transform.position.x > 1500 || transform.position.x < -1500 || transform.position.z < 0 || transform.position.y > 1500 || transform.position.y < -1500){
			Destroy(gameObject);
			spawner.asteroidCount--;
			Debug.Log (name + "Got Destroyed");
		}
	}
}
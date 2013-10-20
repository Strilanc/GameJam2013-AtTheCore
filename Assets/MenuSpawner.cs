using UnityEngine;
using System.Collections;

public class MenuSpawner : MonoBehaviour {
		public Rigidbody Asteroid;
		public int min = -200;
		public int max = 200;
		int randPosition;
		public int asteroidCount = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		randPosition = Random.Range(min, max);
		if (asteroidCount > 10){
			Instantiate(Asteroid, new Vector3(randPosition, randPosition, 200), Quaternion.identity);
			asteroidCount += 1;
		}		
	}
}

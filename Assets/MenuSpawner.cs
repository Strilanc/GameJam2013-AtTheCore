using UnityEngine;
using System.Collections;

public class MenuSpawner : MonoBehaviour {
		public GameObject Asteroid;
		public int min = -1500;
		public int max = 1500;
		public float randPosition;
		public int asteroidCount = 0;
		public int minDist = 10;
		public int maxDist = 300;
		public int ranDist;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		ranDist = Random.Range(minDist, maxDist);
		randPosition = Random.Range(min, max);
		
		if (asteroidCount < 30){
			GameObject Go = Instantiate(Asteroid, new Vector3(randPosition, randPosition, ranDist), transform.rotation) as GameObject;
			asteroidCount++;
			Go.AddComponent<menuRoid>();
			Go.GetComponent<menuRoid>().spawner = this;
		}		
	}
}

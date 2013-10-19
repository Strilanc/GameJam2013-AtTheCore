using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataModel : MonoBehaviour {
	
	public GameObject asteroid;
	public GameObject shipt;
	
	public Dictionary<long,GameObject> allThings = new Dictionary<long, GameObject>();
	
	public void UpdateObject(long id){
		GameObject go = allThings[id];
		Ship ship  = go.GetComponent<Ship>();
		Asteroid asteroid  = go.GetComponent<Asteroid>();
		
	}
	
	
	
	public void CreateWorld(int id){
        //GameObject obj = Instantiate(shipt);
		
	}
	
	
	
	public void CreateAsteroid(){
		
	}
}

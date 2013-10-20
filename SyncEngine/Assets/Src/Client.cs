using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;



public class Client : MonoBehaviour {
	
	public int frameCount = 0;


	//public Int32 dirty = 0;
	public DataModel dataModel;
	
	public String prefab;

	public List<GameObject> objs = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < 10; i++){
			Debug.Log ("ENGER");
		GameObject go = dataModel.CreateSphere(this, prefab);
		objs.Add(go);
		}
		
	}
	
	

	
	// Update is called once per frame
	void Update () {
		

		//if( prefab == "GreenSphere"){
		
			foreach( var obj in objs){
				if ( UnityEngine.Random.value < 0.01){
					var metaData = (MetaData) obj.GetComponent("MetaData");
					var dataItem = dataModel.GetItem(metaData);
					//dataItem.position = new Vector3(UnityEngine.Random.value*6 -3, UnityEngine.Random.value*6 -3, 0) ;
					dataItem.velocity = new Vector3(UnityEngine.Random.value*6 -3, UnityEngine.Random.value*6 -3, 0) ;
					dataModel.UpdateItem(dataItem);
				}
		
			}
		//}
		
		if( prefab == "RedSphere"){
			if (UnityEngine.Random.value > 0.005){
				
				int index =  (int)UnityEngine.Random.value* objs.Count;
				//dataModel.deleteItem();
			}
			
			
		}
		
		
		
		
	}

	
}

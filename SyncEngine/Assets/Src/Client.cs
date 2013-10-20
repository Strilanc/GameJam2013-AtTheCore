using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;



public class Client : MonoBehaviour {
	
	public int frameCount = 0;

	public int nextUid= 0;

	//public Int32 dirty = 0;
	public DataModel dataModel;
	
	public String prefab;

	public List<GameObject> objs = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < 2; i++){
			Debug.Log ("ENGER");
		GameObject go = CreateSphere();
		dataModel.register (go);
		objs.Add(go);
		}
		
	}
	
	
	public GameObject CreateSphere(){
		GameObject obj = (GameObject)Instantiate(Resources.Load(prefab));
		
		
		obj.transform.parent = this.transform;
		obj.transform.position = new Vector3(UnityEngine.Random.value*70 -35, UnityEngine.Random.value*26 -13, 0) ;
		obj.AddComponent("MetaData");
		
		var metaData = (MetaData) obj.GetComponent("MetaData");
		//if( metaData != null){
			metaData.uid = nextUid++;
			metaData.dataModel = dataModel;
			metaData.client = this;
			metaData.gameObj = obj;

		//}
		 return obj;
	}
	
	// Update is called once per frame
	void Update () {
		

		if( prefab == "GreenSphere"){
		
			foreach( var obj in objs){
				if ( UnityEngine.Random.value < 0.001){
					var metaData = (MetaData) obj.GetComponent("MetaData");
					var dataItem = dataModel.GetItem(metaData);
					dataItem.velocity = new Vector3(UnityEngine.Random.value*6 -3, UnityEngine.Random.value*6 -3, 0) ;
					dataModel.UpdateItem(dataItem);
				}
		
			}
		}
		
		
	}

	
}

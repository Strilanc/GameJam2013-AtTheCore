using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;


public class DataItem {
	public int uid;
	public Vector3 position = new Vector3();
	public Vector3 velocity = new Vector3();
	public bool isDeleted;



	
	public void Initialise (GameObject client) {
		
		position = client.transform.position;
		var metaData = client.GetComponent<MetaData>();
		uid = metaData.uid;
		velocity = metaData.velocity;
		isDeleted = false;
	}
	
}

public class DataModel : MonoBehaviour {
	public SyncEngine syncEngine;
	public Dictionary<int ,DataItem> allThings = new Dictionary<int, DataItem>();
		
	public Dictionary<int , GameObject> uidObjectMap = new Dictionary<int, GameObject>();
	public Queue<int> updatedItems = new Queue<int>();
	public Queue<int> newItems = new Queue<int>();
	
	public Client client;  // Probably not need after testing.
	
	public int nextUid;	

 
	// # Do we need client ID?
	public void register (GameObject go) {
	    var metaData = go.GetComponent<MetaData>();
		
		var item = new DataItem ();
		item.Initialise (go);
		
		string bits = Convert.ToString(metaData.uid,2);
		Debug.Log ("QQ: " + bits);
		Debug.Log ("QQE: " + metaData.uid);
		
		allThings.Add(metaData.uid, item);
		uidObjectMap[metaData.uid] = go;
	}
	
	public void addItem(DataItem di){
		Debug.Log ("ADDD " + di.uid + client.prefab);
		allThings[di.uid] = di;
		//uidObjectMap[di.uid] = go;
		newItems.Enqueue(di.uid);
	}
	
	public void _LocalUpdateItem (DataItem item) {
		// lock this shit otherwise the network item will diverge.
		//Debug.Log ("updated" + item.did);
		Debug.Log ("WAS: " +allThings [item.uid].position.x + "  NEXT:" + item.position.x  + "  " + client.prefab + " " + item.uid) ;
		allThings [item.uid] = item;
	//Debug.Log("updatedItems: "+ updatedItems.Count + "  "+ client.prefab);
		updatedItems.Enqueue(item.uid);
	}
	
	public void UpdateItem (DataItem item) {
		_LocalUpdateItem(item);
		allThings [item.uid] = item;
		
		syncEngine.syncRemotely(item);
	}
	
	public DataItem GetItem (int id) {
		return allThings[id];
	}
	public DataItem GetItem (MetaData client) {
		var metaData = client.GetComponent<MetaData>();
		return GetItem (metaData.uid);
	}
	
	public bool DeleteItem(MetaData md){
		int uid = md.uid;
		DataItem di = allThings[uid];
		di.isDeleted = true;
		allThings.Remove(uid);
		uidObjectMap.Remove(uid);	
		return false;
	}
	

	public void Update(){

		
		while(newItems.Count > 0){
			int uId = newItems.Dequeue();
			Debug.Log ("GENERATE:" + uId + " "+ client.prefab);
			//CreateSphere(cl
			GameObject go = CreateSphereWithUid(client,client.prefab,uId);
			
			var metaData = go.GetComponent<MetaData>();
			uidObjectMap[uId] = go;
			
			Interlocked.Exchange(ref metaData.dirty, 1);
		}
		while(updatedItems.Count > 0){
			int uId = updatedItems.Dequeue();
			//Debug.Log("UPDATE::: " + uId + " "  + client.prefab);
			try{
			var metaData = uidObjectMap[uId].GetComponent<MetaData>();
			Interlocked.Exchange(ref metaData.dirty, 1);
			}catch(Exception e){
				Debug.LogWarning ("ID: " + uId + " Not found");	
				//newItems.Enqueue(uId);
			}
		}
		
	
		
	}
	
	
	public GameObject CreateSphere(Client client,String prefab){
		GameObject obj = (GameObject)Instantiate(Resources.Load(prefab));
		
		
		obj.transform.parent = client.transform;
		obj.transform.position = new Vector3(UnityEngine.Random.value*70 -35, UnityEngine.Random.value*26 -13, 0) ;
		var metaData = obj.AddComponent<MetaData>();
		
		metaData.uid = nextUid++;
		metaData.dataModel = this;
		metaData.client = client;
		metaData.gameObj = obj;
		register(obj);
		 return obj;
	}
	
	
	public GameObject CreateSphereWithUid(Client client,String prefab,int uid){
		GameObject obj = (GameObject)Instantiate(Resources.Load(prefab));
		
		
		obj.transform.parent = client.transform;
		obj.transform.position = new Vector3(UnityEngine.Random.value*70 -35, UnityEngine.Random.value*26 -13, 0) ;
			var metaData = obj.AddComponent<MetaData>();
		
		metaData.uid = uid;
		metaData.dataModel = this;
		metaData.client = client;
		metaData.gameObj = obj;
	

		 return obj;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;


public class DataItem {
	public int did;
	public Vector3 position = new Vector3();
	public Vector3 velocity = new Vector3();

	
	public void Initialise (GameObject client, int _did) {
		did = _did;
		position = client.transform.position;
		var metaData = client.GetComponent<MetaData>();
		velocity = metaData.velocity;
	}
	
}

public class DataModel : MonoBehaviour {
	public SyncEngine syncEngine;
	public Dictionary<int ,DataItem> allThings = new Dictionary<int, DataItem>();
		
	public Dictionary<int ,int> didToClientId = new Dictionary<int, int>();
	public Dictionary<int , GameObject> clientIdMap = new Dictionary<int, GameObject>();
	public Queue<int> updatedItems = new Queue<int>();
	
	int nextdid = 0;

 
	
	public void register (GameObject go) {
		var item = new DataItem ();
		item.Initialise (go, nextdid++);
	    var metaData = go.GetComponent<MetaData>();
		int clientId = metaData.uid;
		
		didToClientId [item.did] = clientId;
		
		string bits = Convert.ToString(clientId,2);
		Debug.Log ("QQ: " + bits);
		allThings.Add(clientId, item);
		clientIdMap[clientId] = go;
	}
	
	public void _LocalUpdateItem (DataItem item) {
		// lock this shit otherwise the network item will diverge.
		//Debug.Log ("updated" + item.did);
		var clientId = didToClientId[item.did];
		allThings [clientId] = item;
	Debug.Log("updatedItems: "+ updatedItems.Count);
		updatedItems.Enqueue(clientId);
	}
	
	public void UpdateItem (DataItem item) {
		_LocalUpdateItem(item);
		var clientId = didToClientId[item.did];
		allThings [clientId] = item;
		
		syncEngine.syncRemotely(item);
	}
	
	public DataItem GetItem (int id) {
		return allThings[id];
	}
	public DataItem GetItem (MetaData client) {
		var metaData = client.GetComponent<MetaData>();
		int clientId = metaData.uid;
		return GetItem (clientId);
	}
	

	public void Update(){
		while(updatedItems.Count > 0){
			int uId = updatedItems.Dequeue();	
			var metaData = clientIdMap[uId].GetComponent<MetaData>();
			Interlocked.Exchange(ref metaData.dirty, 1);
		}
	}
}

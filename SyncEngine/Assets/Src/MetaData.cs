using UnityEngine;
using System.Collections;
using System.Threading;

public class MetaData : MonoBehaviour {

	public int uid;
	public Vector3 velocity = new Vector3(0,0,0);
	public DataModel dataModel; 
	public GameObject gameObj;
	public bool isActive = false;
	public Client client;
	// Use this for initialization
	
	public int dirty = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var item = dataModel.GetItem (this);
		
		// reset if needed.
		if( Interlocked.CompareExchange(ref dirty, 0, 1) == 1)	{
			transform.position =  item.position;
			velocity = item.velocity;
		}

		//Debug.Log ("VAL:"+(isActive==true));
		if(isActive){
			ProcessInput();	
		}

		transform.Translate(velocity * Time.deltaTime);
	}
	void saveDataToDataModel() {
		DataItem dm = new DataItem();
		dm.did = uid;
		dm.position = gameObj.transform.position;
		dm.velocity = velocity;
		
		dataModel.UpdateItem(dm);
	}
	
	void ProcessInput(){
		//Debug.Log ("IN");
		float dV= 0.25f;
		
		DataItem d;
		if(Input.anyKeyDown){
			d = dataModel.GetItem(this);
		
			if (Input.GetKeyDown (KeyCode.W)){
				d.velocity.y += dV;
			}else if(Input.GetKeyDown (KeyCode.S)){
				d.velocity.y += -dV;
			}else if(Input.GetKeyDown (KeyCode.D)){
				d.velocity.x += dV;
			}else if(Input.GetKeyDown (KeyCode.A)){
				d.velocity.x += -dV;	
			}
		
			Debug.Log ("PINTPU");
			saveDataToDataModel();
		}
	}
}

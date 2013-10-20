using UnityEngine;
using System.Collections;
using System;
public class MegaControl : MonoBehaviour {
	
	public Mcilhargey start;
	public Mcilhargey quit;
	
	public Vector3 PositionAtDepth{
		get{
			return new Vector3(Input.mousePosition.x,Input.mousePosition.y,Mathf.Infinity);
		}
	}
	
	void Update () {
		
		
		
		Ray ray = Camera.main.ScreenPointToRay(PositionAtDepth);
		
		RaycastHit hit;
		if(Physics.Raycast(ray.origin,ray.direction,out hit) && Input.GetMouseButton(0)){
			Mcilhargey mc = hit.transform.GetComponent<Mcilhargey>();
			if(mc != null){
				if(mc.action == "start"){
					Application.LoadLevel("Scene1");
				}
			}
			
		}
		
		if(Physics.Raycast(ray.origin,ray.direction,out hit)){
			Mcilhargey mc = hit.transform.GetComponent<Mcilhargey>();
			if(mc != null){
				mc.guiTEx.color = Color.blue;
			}
			
		}else{
			start.guiTEx.color = Color.white;
				quit.guiTEx.color = Color.white;
		}
	}
}

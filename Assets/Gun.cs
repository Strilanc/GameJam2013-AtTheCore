using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public GameObject projectile;
	public AudioSource gunSound;
	public bool shoot;
	public bool previous;
	
	public void Update(){
		
		if(shoot && previous == false){
			gunSound.Play();
		}else if(!shoot && previous == true) {
			gunSound.Stop();
		}
		
		if(shoot){
			var go = (GameObject)Instantiate(projectile,transform.position,transform.rotation);
			go.rigidbody.velocity = transform.parent.rigidbody.velocity +transform.forward*1000;
		}
		previous = shoot;
	}
}

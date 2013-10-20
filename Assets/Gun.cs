using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public GameObject projectile;
	public AudioSource gunSound;
	public bool shoot;
	public bool previous;
	public float timer;
	public float shotTime = 0.2f;
	public void Update(){
		
		if(shoot && previous == false){
			timer = Time.time;
			gunSound.Play();
		}else if(!shoot && previous == true) {
			gunSound.Stop();
		}
		
		if(shoot && Time.time - timer > shotTime){
			timer = Time.time;
			var go = (GameObject)Instantiate(projectile,transform.position,transform.rotation);
			go.rigidbody.velocity = transform.parent.rigidbody.velocity +transform.forward*250;
		}
		previous = shoot;
    }
}

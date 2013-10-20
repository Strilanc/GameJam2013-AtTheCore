using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public GameObject projectile;
	public void Update(){
		Instantiate(projectile,transform.position,transform.rotation);
	}
}

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public IEnumerator LifeCycle(){
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}
	
	public void Start(){
		StartCoroutine(LifeCycle());
	}
}

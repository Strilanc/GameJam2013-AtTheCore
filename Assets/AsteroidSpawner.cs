using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {
    private float nextSpawn = 0.0f;
    public GameObject ThingToInstantiate;
    public Ship Ship;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //nextSpawn -= Time.deltaTime;

        while (nextSpawn < 0) {
            nextSpawn += Random.Range(0f, 5.0f);
            
            var t1 = Random.Range(0f, 2 * Mathf.PI);
            var t2 = Random.Range(-Mathf.PI/2, Mathf.PI/2);
            var v1 = new Vector3(Mathf.Cos(t1)*Mathf.Cos(t2), Mathf.Sin(t1)*Mathf.Cos(t2), Mathf.Sin(t2))*100;
            var x = (GameObject)Instantiate(ThingToInstantiate, Ship.transform.position + v1, Quaternion.identity);
            var r = x.GetComponent<Rigidbody>();
            r.velocity = -v1/5 + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) + Ship.rigidbody.velocity;
            r.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
	}
}

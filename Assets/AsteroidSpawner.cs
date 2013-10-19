using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {
    private float nextSpawn = 0.0f;
    public GameObject ThingToInstantiate;
    public Ship Ship;
    private readonly List<GameObject> _asteroids = new List<GameObject>(); 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    var vel = Ship.rigidbody.velocity;
	    var pos = Ship.transform.position;
	    var antiVel = Mathf.Clamp(Mathf.Lerp(1000, 0, pos.magnitude/1000), 0, 1000) * pos.normalized;
	    var dragVel = vel - antiVel;
	    var sweptVolume = dragVel.magnitude*Time.deltaTime;

        nextSpawn -= sweptVolume;

        foreach (var a in _asteroids.ToArray()) {
            if ((a.transform.position - Ship.transform.position).magnitude > 5000) {
                DestroyObject(a.gameObject);
                _asteroids.Remove(a);
            }
        }
        while (nextSpawn < 0 && _asteroids.Count < 100) {
            nextSpawn += Random.Range(0f, 100f);

            var spawnDir = dragVel.normalized * 1000;
            var xx = Vector3.Cross(Vector3.up, spawnDir).normalized;
            if (xx.magnitude < 0.001) xx = Vector3.Cross(Vector3.forward, spawnDir).normalized;
            if (xx.magnitude < 0.001) xx = Vector3.Cross(Vector3.right, spawnDir).normalized;
            var yy = Vector3.Cross(Vector3.up, xx).normalized;

            var v1 = spawnDir + xx*Random.Range(0f, 1000f) + yy*Random.Range(0f, 1000f);
            var x = (GameObject)Instantiate(ThingToInstantiate, Ship.transform.position + v1.normalized*1000f, Quaternion.identity);
            var r = x.GetComponent<Rigidbody>();
            r.velocity = antiVel.normalized*5 + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            r.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            _asteroids.Add(x);
        }
	}
}

using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {
    private float nextSpawn = 0.0f;
    public List<GameObject> ThingToInstantiate;
    public Ship Ship;
    private readonly List<GameObject> _asteroids = new List<GameObject>(); 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        nextSpawn -= Time.deltaTime;

        foreach (var a in _asteroids.ToArray()) {
            if ((a.transform.position - Ship.transform.position).magnitude > 5000) {
                DestroyObject(a.gameObject);
                _asteroids.Remove(a);
            }
        }
        while (nextSpawn < 0 && _asteroids.Count < 1000) {
            nextSpawn += Random.Range(0f, 0.1f);

            Vector3 pos;
            int i = 0;
            do {
                var spawnDir = -Ship.transform.position.normalized;
                var xx = Vector3.Cross(Vector3.up, spawnDir).normalized;
                if (xx.magnitude < 0.001) xx = Vector3.Cross(Vector3.forward, spawnDir).normalized;
                if (xx.magnitude < 0.001) xx = Vector3.Cross(Vector3.right, spawnDir).normalized;
                var yy = Vector3.Cross(spawnDir, xx).normalized;

                var v1 = spawnDir + xx*Random.Range(0f, 1f) + yy*Random.Range(0f, 1f);
                pos = Ship.transform.position + v1.normalized*1000f;
                i += 1;
            } while (Physics.CheckSphere(pos, 200) && i < 10);
            if (i == 10) continue;
            var t = Random.Range(0, ThingToInstantiate.Count);
            var x = (GameObject)Instantiate(ThingToInstantiate[t], pos, Quaternion.identity);
            var r = x.GetComponent<Rigidbody>();
            r.interpolation = RigidbodyInterpolation.Interpolate;
            r.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            x.transform.localScale *= Random.Range(0.5f, 1.5f);
            r.velocity = r.transform.position.normalized*5 + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            r.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            _asteroids.Add(x);
        }
	}
}

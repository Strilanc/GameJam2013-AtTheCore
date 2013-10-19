using UnityEngine;
using System.Collections;

public class SpeedStreak : MonoBehaviour {
    public float life;
	// Use this for initialization
	void Start () {
	    life = Random.Range(0f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
        life += Time.deltaTime;
        //if (life > 1) Destroy(this.gameObject);
	}
}

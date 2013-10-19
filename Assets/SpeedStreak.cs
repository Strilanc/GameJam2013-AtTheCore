using UnityEngine;
using System.Collections;

public class SpeedStreak : MonoBehaviour {
    public float life;
	void Start () {
	    life = Random.Range(0f, 10f);
	}
	void Update () {
        life += Time.deltaTime;
	}
}

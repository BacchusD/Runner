using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour {

	public float speed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var move = new Vector3 (speed, 0, 0) * Time.deltaTime;
		gameObject.transform.position += move;
	}
}

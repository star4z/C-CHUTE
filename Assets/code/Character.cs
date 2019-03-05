using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public bool EnteredTrigger;
	public GameObject CollisionWith;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		EnteredTrigger = true;
		CollisionWith = other.gameObject;
		// Debug.Log(CollisionWith.name + " collided with " + name);
	}

	public void OnTriggerHandled(){
		EnteredTrigger = false;
	}
}
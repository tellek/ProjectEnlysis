using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour {

	public float gravity = 3.0f;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		//rb.velocity.y += gravity * Time.deltaTime;
		rb.AddForce(gravity * Time.deltaTime, 0, 0, ForceMode.Force);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFireAnimation : MonoBehaviour {

    public Animator ShootAnimator;

	// Use this for initialization
	void Start () {
        ShootAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            ShootAnimator.Play("VacuumEagle_Fire");
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSameSizeOnScreen : MonoBehaviour {

    public float Divisor = 0.2f;

    private Vector3 desiredScale = new Vector3(1, 1, 1);
    private float reducer = 1000;
	
	void Update () {
        if (Camera.main == null) return;
        transform.localScale = desiredScale * (Vector3.Distance(transform.position, Camera.main.transform.position) / reducer) / Divisor;
    }
}

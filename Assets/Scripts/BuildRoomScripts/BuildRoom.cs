using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildRoom : MonoBehaviour {
    public Camera MainCamera;
    public Camera BuildRoomCamera;

	// Use this for initialization
	void Start () {
        MainCamera.enabled = true;
        BuildRoomCamera.enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("c"))
        {
            MainCamera.enabled = !MainCamera.enabled;
            BuildRoomCamera.enabled = !BuildRoomCamera.enabled;

            GlobalVariables.BuildRoomIsActive = BuildRoomCamera.enabled;
        }
	}
}

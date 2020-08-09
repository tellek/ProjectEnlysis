using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFps : MonoBehaviour {

    private float deltaTime = 0.0f;

	// Update is called once per frame
	void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Performance: " + text);
    }
}

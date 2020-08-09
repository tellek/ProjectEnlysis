﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour {

    void Update () {
        if (Camera.main == null) return;
        transform.LookAt(Camera.main.transform);
    }
}

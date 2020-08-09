using Assets.Scripts.Constants;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float ThrustSpeed = 2f;
    public float BoostSpeed = 4f;
    public float RotateSpeed = 2f;
    [Space]
    [Range(0.01f, 0.50f)]
    public float XRotationDistance = 0.10f;
    [Range(0.01f, 0.50f)]
    public float YRotationDistance = 0.10f;
    [Range(0.01f, 0.50f)]
    public float ZRotationDistance = 0.10f;
    [Range(0.01f, 0.25f)]
    public float RatationSmoothing = 0.25f;
    public Transform ship;

    private Rewired.Player player;

    void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        Debug.Log(player.GetAxis(Inputs.ForwardAxis));
        //if (Inputs.HorizontalAxis != "0") Debug.Log(player.GetAxis(Inputs.HorizontalAxis));
        //if (Inputs.VerticalAxis != "0") Debug.Log(player.GetAxis(Inputs.VerticalAxis));
        //if (Inputs.VerticalRotation != "0") Debug.Log(player.GetAxis(Inputs.VerticalRotation));
        //if (Inputs.HorizontalRotation != "0") Debug.Log(player.GetAxis(Inputs.HorizontalRotation));
        //if (Inputs.BoostButton != "0") Debug.Log(player.GetAxis(Inputs.BoostButton));
    }
}

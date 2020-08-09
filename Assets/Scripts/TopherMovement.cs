using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// Topher's Ship Movement
/// </summary>
public class TopherMovement : MonoBehaviour
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

    private Rewired.Player player;
    private Transform ship;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        ship = transform.Find("Ship");
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Move the object forward along its z axis 1 unit/second.
        transform.Translate(Vector3.forward * player.GetAxis("ZThrust")); // * Time.deltaTime
        transform.Translate(Vector3.left * (player.GetAxis("XThrust") * -1));
        transform.Translate(Vector3.up * player.GetAxis("YThrust")); // * Time.deltaTime

        //Vector3 newPos = GetPlayerPosition();
        //transform.position = newPos;

        Quaternion newRot = GetShipRotation();
        ship.localRotation = Quaternion.Lerp(ship.localRotation, newRot, RatationSmoothing);

        //Quaternion eulerRot = Quaternion.Euler(
        //    transform.rotation.x + (player.GetAxis("YRotation") * 5), 
        //    transform.rotation.y + (player.GetAxis("XRotation") * 5), 
        //    0.0f);

        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(player.GetAxis("YRotation"), player.GetAxis("XRotation"), 0);

        //transform.rotation = Quaternion.Slerp(transform.rotation, eulerRot, Time.deltaTime * 10);
    }

    //private Quaternion GetPlayerRotation()
    //{

    //}
    
    private Vector3 GetPlayerPosition()
    {
        float boost = 1;
        if (player.GetButton("Boost"))
        {
            boost = BoostSpeed;
        }
        if (player.GetButtonDown("Boost") && player.GetAxis("ZThrust") > 0f)
        {
            player.SetVibration(0, 0.15f);
        }
        if (player.GetButtonUp("Boost"))
        {
            player.StopVibration();
        }

        return new Vector3(
                transform.position.x + player.GetAxis("XThrust"),
                transform.position.y + player.GetAxis("YThrust"),
                transform.position.z + (player.GetAxis("ZThrust") * boost));

        
    }

    private Quaternion GetShipRotation()
    {
        Quaternion newRot = ship.rotation;
        if (player.GetAxis("YThrust") > 0.5f)
        {
            newRot.x = (XRotationDistance * -1);
        }
        else if (player.GetAxis("YThrust") < -0.5f)
        {
            newRot.x = XRotationDistance;
        }
        else
        {
            newRot.x = 0f;
        }

        if (player.GetAxis("XThrust") > 0.5f)
        {
            newRot.y = YRotationDistance;
            newRot.z = (YRotationDistance * -1);

        }
        else if (player.GetAxis("XThrust") < -0.5f)
        {
            newRot.y = (ZRotationDistance * -1);
            newRot.z = ZRotationDistance;
        }
        else
        {
            newRot.y = 0f;
            newRot.z = 0f;
        }
        return newRot;
    }
}

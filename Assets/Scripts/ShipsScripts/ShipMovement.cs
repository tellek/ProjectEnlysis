using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float reverseSpeed = 5f;
    public float strafeSpeed = 5f;
    public float boostMultiplier = 2.0f;
    public float thrustPower = 5.0f;
    public float rotateSpeed = 2.5f;

    private float actualMoveSpeed = 0;
    private float horizontalAxis = 0;
    private float verticalAxis = 0;
    private bool hasTarget = false;
    private bool isStrafing = false;
    private bool isMovingFoward = false;

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        hasTarget = gameObject.GetComponent<FindTarget>().haveTarget;

        // Rotation.   
        if (!isStrafing && !hasTarget)
        {
            var x = horizontalAxis * Time.deltaTime * rotateSpeed;
            transform.Rotate(0, x, 0);
        }

        // Forward/Backward thrust.
        if (!Input.GetKey("space"))
        {
            var z = verticalAxis * Time.deltaTime * actualMoveSpeed;
            transform.Translate(0, 0, z);
        }

        // Begin forward thrust.
        if (Input.GetKeyDown("w") && !Input.GetKey("space"))
        {
            isStrafing = false;
            actualMoveSpeed = moveSpeed;
            isMovingFoward = true;
        }
        // End forward thrust.
        if (Input.GetKeyUp("w") && !Input.GetKey("space"))
        {
            isMovingFoward = false;
            actualMoveSpeed = 0;
        }

        // Begin reverse thrust.
        if (Input.GetKeyDown("s") && !Input.GetKey("space"))
        {
            actualMoveSpeed = reverseSpeed;
        }
        // End reverse thrust.
        if (Input.GetKeyUp("s") && !Input.GetKey("space"))
        {
            actualMoveSpeed = moveSpeed;
        }

        // Boost
        if (isMovingFoward && !Input.GetKey("space"))
        {
            if (Input.GetKey("left shift"))
            {
                actualMoveSpeed = moveSpeed * boostMultiplier;
            }
            else actualMoveSpeed = moveSpeed;
        }

        // Strafing
        if (Input.GetKey("left shift") && !isMovingFoward)
        {
            isStrafing = true;
            var strafe = horizontalAxis * Time.deltaTime * strafeSpeed;
            transform.Translate(strafe, 0, 0);
        }
        if (Input.GetKeyUp("left shift"))
        {
            isStrafing = false;
        }

        // Targetted Thrusting
        if (hasTarget)
        {
            isStrafing = true;
            var strafe = horizontalAxis * Time.deltaTime * strafeSpeed;
            transform.Translate(strafe, 0, 0);
        }
        if (!hasTarget)
        {
            isStrafing = false;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 15, Screen.width, Screen.height), "Speed: " + actualMoveSpeed.ToString() + " (h:" + horizontalAxis.ToString() 
            + ")(v:" + verticalAxis.ToString() + ")");
    }
}

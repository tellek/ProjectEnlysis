using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBasedMovement : MonoBehaviour {

    public float forwardSpeed = 100f;
    public float reverseSpeed = 75f;
    public float strafeSpeed = 100f;
    public float thrustMultiplier = 3.0f;
    public float boostPower = 400f;

    private float horizontalAxis = 0;
    private float verticalAxis = 0;
    private Rigidbody rb;
    private bool hasTarget = false;
    private Vector3 currentVelocity = Vector3.zero;
    private FindTarget targeting;

    bool doBoost = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targeting = gameObject.GetComponent<FindTarget>();
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        hasTarget = targeting.haveTarget;
        currentVelocity = rb.velocity;

        if (Input.GetKeyDown("space")) doBoost = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKey("a"))
        {
            if (doBoost)
            {
                if (Input.GetKey("w") || Input.GetKey("s"))
                    rb.AddRelativeForce(-boostPower / 2, 0, 0, ForceMode.Impulse);
                else rb.AddRelativeForce(-boostPower, 0, 0, ForceMode.Impulse);

                //if (hasTarget)
                //{
                //    rb.AddForce(transform.forward * thrustPower * forwardThrustMultiplier, ForceMode.Impulse);
                //}
            }
            else
            {
                rb.AddRelativeForce(horizontalAxis * strafeSpeed, 0, 0, ForceMode.Acceleration);
            }
        }

        if (Input.GetKey("d"))
        {
            if (doBoost)
            {
                if (Input.GetKey("w") || Input.GetKey("s"))
                    rb.AddRelativeForce(boostPower / 2, 0, 0, ForceMode.Impulse);
                else rb.AddRelativeForce(boostPower, 0, 0, ForceMode.Impulse);

                //if (hasTarget)
                //{
                //    rb.AddForce(transform.forward * thrustPower * forwardThrustMultiplier, ForceMode.Impulse);
                //}
            }
            else
            {
                rb.AddRelativeForce(horizontalAxis * strafeSpeed, 0, 0, ForceMode.Acceleration);
            }
        }

        if (Input.GetKey("s"))
        {
            if (doBoost)
            {
                if (Input.GetKey("d") || Input.GetKey("a"))
                    rb.AddRelativeForce(0, 0, -boostPower / 2, ForceMode.Impulse);
                else rb.AddRelativeForce(0, 0, -boostPower, ForceMode.Impulse);
            }
            else
            {
                rb.AddRelativeForce(0, 0, verticalAxis * reverseSpeed, ForceMode.Acceleration);
            }
        }

        if (Input.GetKey("w"))
        {
            if (doBoost)
            {
                if (Input.GetKey("d") || Input.GetKey("a"))
                    rb.AddRelativeForce(0, 0, boostPower / 2, ForceMode.Impulse);
                else rb.AddRelativeForce(0, 0, boostPower, ForceMode.Impulse);
            }
            else
            {
                if (Input.GetKey("left shift"))
                {
                    rb.AddRelativeForce(0, 0, (verticalAxis * forwardSpeed) * thrustMultiplier, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddRelativeForce(0, 0, verticalAxis * forwardSpeed, ForceMode.Acceleration);
                }
            }
        }

        doBoost = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 15, Screen.width, Screen.height), "Speed: " + currentVelocity.ToString() + " (h:" + horizontalAxis.ToString()
            + ")(v:" + verticalAxis.ToString() + ")");
    }
}

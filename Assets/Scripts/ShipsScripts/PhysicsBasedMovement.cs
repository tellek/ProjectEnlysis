using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBasedMovement : MonoBehaviour {

    public float forwardSpeed = 100f;
    public float reverseSpeed = 75f;
    public float strafeSpeed = 100f;
    public float thrustMultiplier = 3.0f;
    public float boostPower = 400f;

    public GameObject LeftSensor;
    public GameObject RightSensor;

    private float horizontalAxis = 0;
    private float verticalAxis = 0;
    private Rigidbody rb;
    private bool hasTarget = false;
    private Vector3 currentVelocity = Vector3.zero;
    private FindTarget targeting;

    bool doBoost = false;
    bool wallDetected = true;

    private bool wDown = false;
    private bool aDown = false;
    private bool sDown = false;
    private bool dDown = false;
    private int jumpDir = 0;

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

        if (Input.GetKeyDown("space") && true) doBoost = true;
    }

    void FixedUpdate()
    {
        //Need to add the ability to detect a wall when boosting.
        SetMoveDirection();

        GameObject s = null;
        if (aDown)
        {
            s = LeftSensor;
            jumpDir = 1;
        }
        else if (dDown)
        {
            s = RightSensor;
            jumpDir = 2;
        }
        else jumpDir = 0;
        if (aDown || dDown)
        {
            Vector3 direction = (s.transform.position - transform.position).normalized;
            var sensorRay = new Ray(transform.position, direction);
            Debug.DrawLine(sensorRay.origin, sensorRay.GetPoint(11), Color.red);
            RaycastHit sensorHit;


            if (doBoost)
            {
                //rb.MoveRotation(new Quaternion(0, 90, 0, 0));
                if (jumpDir == 1)
                {
                    transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90);
                }
                if (jumpDir == 2)
                {
                    transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90);
                }
                
                
            }


            if (Physics.Raycast(sensorRay, out sensorHit, 11))
            {
                if (jumpDir == 1)
                {
                    Debug.Log("HIT Left");
                }
                if (jumpDir == 2)
                {
                    Debug.Log("HIT Right");
                }

                if (sensorHit.transform.tag == "TempWall")
                {
                    
                    
                }
            }
        }

        LeftMovement();
        RightMovement();
        ForwardMovement();
        BackwardMovement();

        doBoost = false;
    }

    private void LeftMovement()
    {
        if (Input.GetKey("a"))
        {
            if (doBoost && !wallDetected)
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
    }
    private void RightMovement()
    {
        if (Input.GetKey("d"))
        {
            if (doBoost && !wallDetected)
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
    }
    private void ForwardMovement()
    {
        if (Input.GetKey("w"))
        {
            if (doBoost && !wallDetected)
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
    }
    private void BackwardMovement()
    {
        if (Input.GetKey("s"))
        {
            if (doBoost && !wallDetected)
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
    }

    public void SetMoveDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) wDown = true;
        if (Input.GetKeyDown(KeyCode.A)) aDown = true;
        if (Input.GetKeyDown(KeyCode.S)) sDown = true;
        if (Input.GetKeyDown(KeyCode.D)) dDown = true;
        if (Input.GetKeyUp(KeyCode.W)) wDown = false;
        if (Input.GetKeyUp(KeyCode.A)) aDown = false;
        if (Input.GetKeyUp(KeyCode.S)) sDown = false;
        if (Input.GetKeyUp(KeyCode.D)) dDown = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 15, Screen.width, Screen.height), "Speed: " + currentVelocity.ToString() + " (h:" + horizontalAxis.ToString()
            + ")(v:" + verticalAxis.ToString() + ")");
    }
}

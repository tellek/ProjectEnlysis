using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagDriveV2 : MonoBehaviour
{
    public float HoverHeight = 8f;
    public float HoverForce = 300f;
    public float MagPullStrength = 100f;
    public float RealignSpeed = 0.5f;
    public float VelocityChangeSpeed = 0.5f;
    public float SensorRange = 20f;
    public float Stability = 10f;
    public float StablizationSpeed = 50f;
    public float MaxCorrectionDifference = 50f;
    public float MaxSurfaceSeekDistance = 100f;

    private int moveDirection = 0;

    private FindTarget shipTargeting;
    private Rigidbody rb;
    private Vector3 magUp = new Vector3();
    private Vector3 magDown = new Vector3();
    private Vector3 OldMagDown = new Vector3();
    private bool isMagLocked = false;

    private bool wDown = false;
    private bool aDown = false;
    private bool sDown = false;
    private bool dDown = false;

    private bool magLocked = true;
    
    private bool hasTarget = false;
    private bool magDirectionset = false;
    private Dictionary<string, GameObject> sensors = new Dictionary<string, GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        shipTargeting = gameObject.GetComponent<FindTarget>();
        magUp = transform.up;
        magDown = -transform.up;

        var collection = GameObject.FindGameObjectsWithTag("Sensor")
            .Where(x => x.transform.IsChildOf(transform) && x.name.Contains("Out"));
        foreach (var item in collection)
        {
            sensors.Add(item.name, item);
        }
    }

    private void Update()
    {
        hasTarget = shipTargeting.haveTarget;
    }

    void FixedUpdate()
    {
        ManageSensorArray();
        if (!hasTarget) ManageStabalization();
        ManageMagDrivePull();
        //KeepShipCloseToSurface();
    }

    private void LateUpdate()
    {
        SetMoveDirection();
    }

    void KeepShipCloseToSurface()
    {
        if (true)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxSurfaceSeekDistance))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.cyan);
                if (hit.distance > (HoverHeight + MaxCorrectionDifference))
                {
                    rb.AddForce(magDown * 10, ForceMode.Acceleration);
                }
                else if (hit.distance < (HoverHeight - MaxCorrectionDifference))
                {
                    rb.AddForce(magUp * 10, ForceMode.Acceleration);
                }
            }
        }
    }

    void ManageSensorArray()
    {
        foreach (var sensor in sensors)
        {
            string[] sections = sensor.Key.Split('_');
            var name = sections[0];
            var dir = sections[1];
            var num = sections[2];

            if (Convert.ToInt32(num) != moveDirection) continue;

            Vector3 direction = (sensors[$"{name}_To_{num}"].transform.position - sensor.Value.transform.position).normalized;
            var sensorRay = new Ray(sensor.Value.transform.position, direction);
            Debug.DrawLine(sensorRay.origin, sensorRay.GetPoint(SensorRange), Color.red);

            RaycastHit sensorHit;
            if (Physics.Raycast(sensorRay, out sensorHit, SensorRange))
            {
                if (sensorHit.transform.tag == "MagSurface")
                {
                    magLocked = true;
                    Vector3 reflectVec = sensorHit.normal;

                    float proportionalHeight = (HoverHeight - sensorHit.distance) / HoverHeight;
                    Vector3 actualForce = transform.up * proportionalHeight * HoverForce;

                    rb.AddForce(actualForce, ForceMode.Acceleration);

                    Debug.DrawRay(sensorHit.point, reflectVec, Color.green);
                    SmoothChangeMagDirection(reflectVec);
                }
            }
            else
            {
                magLocked = false;
            }
        }
    }

    void ManageStabalization()
    {
        Vector3 predictedUp = Quaternion.AngleAxis(
                rb.angularVelocity.magnitude * Mathf.Rad2Deg * Stability / StablizationSpeed,
                rb.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, magUp);
        rb.AddTorque(torqueVector * StablizationSpeed * StablizationSpeed);
    }

    void ManageMagDrivePull()
    {
        if (false)
        {
            // Convert vector to local space.
            var locVel = transform.InverseTransformDirection(rb.velocity);
            // Zero out downward velocity.
            locVel.y = 0;
            // Convert vector back to global space.
            rb.velocity = transform.TransformDirection(locVel);
        }
        else
        {
            rb.AddForce(
            magDown.x * MagPullStrength,
            magDown.y * MagPullStrength,
            magDown.z * MagPullStrength,
            ForceMode.Acceleration);
        }
        OldMagDown = magDown;
    }

    bool WasLargeEnoughChange(Vector3 from, Vector3 to)
    {
        if ((from.x - to.x) >= MaxCorrectionDifference) return true;
        if ((from.y - to.y) >= MaxCorrectionDifference) return true;
        if ((from.z - to.z) >= MaxCorrectionDifference) return true;
        return false;
    }

    void SmoothChangeMagDirection(Vector3 newDirection)
    {
        magUp = Vector3.Slerp(magUp, newDirection, RealignSpeed * Time.deltaTime);
        magDown = Vector3.Slerp(magDown, -newDirection, RealignSpeed * Time.deltaTime);
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

        if (wDown && dDown) moveDirection = 45;
        else if (sDown && dDown) moveDirection = 135;
        else if (aDown && sDown) moveDirection = 225;
        else if (wDown && aDown) moveDirection = 315;
        else if (wDown) moveDirection = 0;
        else if (dDown) moveDirection = 90;
        else if (sDown) moveDirection = 180;
        else if (aDown) moveDirection = 270;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Jobs;

public class MagDriveV3 : MonoBehaviour
{
    public float SensorRange = 20f;
    public float ChangeMe = 10f;

    private Dictionary<string, GameObject> sensors = new Dictionary<string, GameObject>();
    private bool magLocked = true;
    private int moveDirection = 0;
    private bool wDown = false;
    private bool aDown = false;
    private bool sDown = false;
    private bool dDown = false;
    private RaycastHit newMagSource;

    void Start()
    {
        var collection = GameObject.FindGameObjectsWithTag("Sensor")
            .Where(x => x.transform.IsChildOf(transform) && x.name.Contains("Out"));
        foreach (var item in collection)
        {
            sensors.Add(item.name, item);
        }
    }

    void Update()
    {
        SetMoveDirection();


    }

    private void FixedUpdate()
    {
        ManageSensorArray();
        Hover();
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
                    newMagSource = sensorHit;

                    Debug.DrawRay(sensorHit.point, newMagSource.normal, Color.green);
                }
            }
            else
            {
                magLocked = false;
            }
        }
    }

    public void Hover()
    {
        var targetPosition = new Vector3(transform.position.x, newMagSource.point.y + 10, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, targetPosition, 0.2f), 10);

        transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.Lerp(transform.up, newMagSource.normal, 0.2f)) * transform.rotation;
        //// Determine which direction to rotate towards
        //var rotateToward = new Vector3(newMagSource.point.x, newMagSource.point.y + ChangeMe, newMagSource.point.z);
        //Vector3 targetDirection = rotateToward - transform.position;

        //// The step size is equal to speed times frame time.
        //float singleStep = 10 * Time.deltaTime;

        //// Rotate the forward vector towards the target direction by one step
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        //// Draw a ray pointing at our target in
        //Debug.DrawRay(transform.position, newDirection, Color.red);

        //// Calculate a rotation a step closer to the target and applies rotation to this object
        //transform.rotation = Quaternion.LookRotation(newDirection);
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

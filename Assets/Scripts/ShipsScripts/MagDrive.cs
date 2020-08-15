using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagDrive : MonoBehaviour {

	public float HoverHeight = 8f;
	public float HoverForce = 300f;
	public float MagPullStrength = 100f;
    public float RealignSpeed = 0.5f;
	public float Stability = 10f;
	public float StablizationSpeed = 50f;
	public List<GameObject> Sensors; //9

	private Rigidbody rb;
	private Vector3 magUp = new Vector3();
    private Vector3 magDown = new Vector3();
    private bool magLocked = true;
    private FindTarget shipTargeting;
    private bool hasTarget = false;

    void Awake () {
		rb = GetComponent<Rigidbody>();
        shipTargeting = gameObject.GetComponent<FindTarget>();
        magUp = transform.up;
        magDown = -transform.up;
    }

    private void Update()
    {
        hasTarget = shipTargeting.haveTarget;
    }

    void FixedUpdate () {
        bool magDirectionset = false;

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, HoverHeight))
        {
            Vector3 reflectVec = hit.normal;
            float proportionalHeight = (HoverHeight - hit.distance) / HoverHeight;
            Vector3 actualForce = this.transform.up * proportionalHeight * HoverForce;
            rb.AddForce(actualForce, ForceMode.Acceleration);

            Debug.DrawLine(this.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, reflectVec, Color.green);

            SmoothChangeMagDirection(reflectVec);
            magDirectionset = true;
        }

        foreach (var sensor in Sensors)
        {
            Vector3 dir = (sensor.transform.position - transform.position).normalized;
            var sensorRay = new Ray(transform.position, dir);
            Debug.DrawLine(sensorRay.origin, sensorRay.GetPoint(15), Color.red);

            RaycastHit sensorHit;
            if (Physics.Raycast(sensorRay, out sensorHit, HoverHeight))
            {
                if (!magDirectionset)
                {
                    Vector3 reflectVec = sensorHit.normal;
                    Debug.DrawRay(sensorHit.point, reflectVec, Color.green);

                    SmoothChangeMagDirection(reflectVec);
                }
                magLocked = true;
            }
            else magLocked = false;
        }

        if (!hasTarget)
        {
            // Stabalization
            Vector3 predictedUp = Quaternion.AngleAxis(
                rb.angularVelocity.magnitude * Mathf.Rad2Deg * Stability / StablizationSpeed,
                rb.angularVelocity) * transform.up;
            Vector3 torqueVector = Vector3.Cross(predictedUp, magUp);
            rb.AddTorque(torqueVector * StablizationSpeed * StablizationSpeed);
        }

        if (magLocked)
        {
            // Drive pull
            rb.AddForce(
            magDown.x * MagPullStrength,
            magDown.y * MagPullStrength,
            magDown.z * MagPullStrength,
            ForceMode.Acceleration);
        }
	}

    void SmoothChangeMagDirection(Vector3 newDirection)
    {
        magUp = Vector3.Slerp(magUp, newDirection, RealignSpeed * Time.deltaTime);
        magDown = Vector3.Slerp(magDown, -newDirection, RealignSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagDrive : MonoBehaviour {

	public float HoverHeight = 5f;
	public float HoverForce = 11f;
	public float MagPullStrength = 10f;
    public float RealignSpeed = 0.5f;
	public float Stability = 0.3f;
	public float StablizationSpeed = 2.0f;
	public List<GameObject> Sensors;

	private Rigidbody rb;
	private Vector3 magUp = new Vector3();
    private Vector3 magDown = new Vector3();
    private bool magLocked = true;

    void Awake () {
		rb = GetComponent<Rigidbody>();
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

            //magUp = reflectVec;
            //magDown = new Vector3(reflectVec.x * -1, reflectVec.y * -1, reflectVec.z * -1);
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

                    //magUp = reflectVec;
                    //magDown = new Vector3(reflectVec.x * -1, reflectVec.y * -1, reflectVec.z * -1);
                    SmoothChangeMagDirection(reflectVec);
                }
                magLocked = true;
            }
            else magLocked = false;
        }

        // Stabalization
        Vector3 predictedUp = Quaternion.AngleAxis(
			rb.angularVelocity.magnitude * Mathf.Rad2Deg * Stability / StablizationSpeed,
			rb.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, magUp);
        rb.AddTorque(torqueVector * StablizationSpeed * StablizationSpeed);

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
        magUp = Vector3.Slerp(magUp, newDirection, 10);
        var negativeDirection = new Vector3(newDirection.x * -1, newDirection.y * -1, newDirection.z * -1);
        magDown = Vector3.Slerp(magDown, negativeDirection, RealignSpeed * Time.deltaTime);
    }

    void EnterZeroGravity()
    {
        rb.
    }
}

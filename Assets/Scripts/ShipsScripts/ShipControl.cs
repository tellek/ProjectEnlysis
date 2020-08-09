using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipControl : MonoBehaviour
{
    // Force of individual thrusters
    public float thrusterForce = 100;
    // Specify the roll rate (multiplier for rolling the ship when steering left/right)	
    public float rollRate = 1.0f;
    // Specify the yaw rate (multiplier for rudder/steering the ship when steering left/right)
    public float yawRate = 1.0f;
    // Specify the pitch rate (multiplier for pitch when steering up/down)
    public float pitchRate = 1.0f;


    private Rigidbody _cacheRigidbody;
    private Transform _cacheTransform;

    void Start()
    {
        _cacheRigidbody = GetComponent<Rigidbody>();
        if (_cacheRigidbody == null) Debug.LogError("Ship has no rigidbody.");

        _cacheTransform = transform;
        if (_cacheTransform == null) Debug.LogError("Spaceship has not been set!");
    }

    void Update()
    {


    }


    void FixedUpdate()
    {
        // Add relative rotational roll torque when steering left/right
        //_cacheRigidbody.AddRelativeTorque(new Vector3(0, 0, -Input.GetAxis("Horizontal") * rollRate));
        // Add rudder yaw torque when steering left/right
        _cacheRigidbody.AddRelativeTorque(new Vector3(0, Input.GetAxis("Horizontal") * yawRate, 0));
        // Add pitch torque when steering up/down
        _cacheRigidbody.AddRelativeTorque(new Vector3(Input.GetAxis("Vertical") * pitchRate, 0, 0));

        // If the thruster is active...
        if (Input.GetButton("Fire1"))
        {
            _cacheRigidbody.AddRelativeForce(Vector3.forward * thrusterForce, ForceMode.Impulse);
        }
    }


}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum EngineState
//{
//    Running,
//    Killed,
//    Charging,
//    Cruising
//}

//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Ship))]
//public class FreelancerStyle : MonoBehaviour
//{
//    const float CRUISE_ACCEL = 50.0f;
//    const float THROTTLE_RAMP_SPEED = 2.0f;
//    const float MULTIPLIER = 1.0f;

//    [Space(10)]
//    public float forwardForce = 48000.0f;
//    public float thrusterForce = 48000.0f;
//    public float strafeForce = 20000.0f;
//    public float reverseFraction = 0.2f;
//    public float linearDrag = 600.0f;

//    [Space(10)]
//    public Vector3 steeringTorque;
//    public Vector3 angularDrag;

//    [Space(10)]
//    public float cruiseSpeed = 300.0f;
//    public float cruiseChargeTime = 5.0f;

//    //[HideInInspector]
//    public EngineState engineState = EngineState.Running;
//    EngineState engineStateLastFrame = EngineState.Running;

//    Ship ship;

//    [Space(10)]
//    public float speed;
//    public float throttle;

//    // Placeholder thruster.
//    const float thrustCapacity = 1000.0f;
//    const float thrustDrain = 165.0f;
//    const float thrustChargeRate = 100.0f;
//    float thrustPower = thrustCapacity;

//    float startingLinearDrag = 600.0f;

//    float targetCruiseThrust = 0.0f;
//    float calculatedTopSpeed = 0.0f;
//    float cruiseStartTime = 0.0f;
//    float cruiseRampTime = 0.0f;

//    bool requestedEngineKill = false;
//    //bool engineKilled = false;

//    public bool IsEngineKilled
//    {
//        get { return (engineState == EngineState.Killed) ? true : false; }
//    }

//    void Awake()
//    {
//        ship = GetComponent<Ship>();
//    }

//    void Start()
//    {
//        thrustPower = thrustCapacity;
//    }

//    public void OnEnable()
//    {
//        startingLinearDrag = linearDrag;
//    }

//    void Update()
//    {
//        UpdateCruiseEngine();

//        // Slide the throttle at a set speed so it's not instantaneous.
//        throttle = Mathf.MoveTowards(throttle, ship.ShipControls.throttle, THROTTLE_RAMP_SPEED * Time.deltaTime);

//        // Recharge the thruster.
//        thrustPower += thrustChargeRate * Time.deltaTime;
//        thrustPower = Mathf.Min(thrustPower, thrustCapacity);

//        // Find if engine kill is requested.
//        requestedEngineKill = ship.ShipControls.engineKill;

//        if (ship.IsPlayer)
//            UpdateFlightUI();
//    }

//    // Update the flight UI as needed.
//    // I'm still uncomfortable that this is happening in this file. It doesn't seem right.
//    private void UpdateFlightUI()
//    {
//        //FlightStatusBar.instance.UpdateSpeed(ship.transform.InverseTransformVector(ship.Rigidbody.velocity).z);
//        //FlightStatusBar.instance.UpdateThrusterPercentage(thrustPower / thrustCapacity);

//        // Update the cruise UI.
//        if (engineState == EngineState.Charging)
//        {
//            //float chargePercent = TimeTools.TimeSince(cruiseStartTime) / cruiseChargeTime;
//            //FlightStatusBar.instance.UpdateCruiseCharging(chargePercent);
//        }
//        else { }
//            //FlightStatusBar.instance.UpdateCruiseCharging(-1.0f);
//    }

//    void UpdateCruiseEngine()
//    {
//        // React to the controls requesting cruise or not.
//        // Went from not cruising to wanting cruise. Charge the engine.
//        if (ship.ShipControls.cruise && (engineState == EngineState.Running || engineState == EngineState.Killed))
//        {
//            engineState = EngineState.Charging;
//            ship.engineTrail.SetTrailState(engineState);
//            cruiseStartTime = Time.time;
//        }

//        // Cruise engine is charging. Start cruising once charge time is finished.
//        //else if (ship.ShipControls.cruise && engineState == EngineState.Charging && TimeTools.TimeSince(cruiseStartTime) >= cruiseChargeTime)
//        //{
//        //    engineState = EngineState.Cruising;
//        //    CruiseStart();
//        //    ship.engineTrail.SetTrailState(engineState);
//        //}

//        // Went from any cruise/charging to normal engines.
//        else if (!ship.ShipControls.cruise && (engineState == EngineState.Cruising || engineState == EngineState.Charging))
//        {
//            engineState = EngineState.Running;
//            if (ship.engineTrail)
//                ship.engineTrail.SetTrailState(engineState);
//        }

//        // Cruise engine is cruising. Make sure to always calculate target cruise thrust in case drag changes.
//        else if (engineState == EngineState.Cruising)
//        {
//            // To derive cruise thrust, use the simple drag equation that dictates top speed.
//            // Subtract it from the normal thrust because cruise is assisting an engine that's already
//            // running at full power. This also makes it so the ramp up to full cruise speed starts at
//            // whatever the normal top speed is.
//            calculatedTopSpeed = forwardForce / linearDrag;
//            targetCruiseThrust = (cruiseSpeed - calculatedTopSpeed) * linearDrag;
//            cruiseRampTime = (cruiseSpeed - calculatedTopSpeed) / CRUISE_ACCEL;
//        }

//        engineStateLastFrame = engineState;
//    }

//    void CruiseStart()
//    {
//        cruiseStartTime = Time.time;
//    }

//    float CruiseRamp()
//    {
//        float rampingForwardCruiseForce;

//        rampingForwardCruiseForce = Mathf.InverseLerp(cruiseStartTime, cruiseStartTime + cruiseRampTime, Time.time);
//        rampingForwardCruiseForce = Mathf.Clamp01(rampingForwardCruiseForce) * targetCruiseThrust;
//        rampingForwardCruiseForce += forwardForce;

//        return rampingForwardCruiseForce;
//    }

//    void FixedUpdate()
//    {
//        // Rigidbody drag and gravity MUST be zero.
//        ship.Rigidbody.drag = 0.0f;
//        ship.Rigidbody.angularDrag = 0.0f;
//        ship.Rigidbody.useGravity = false;

//        // Engine kill can only be on when the ship is under no input other than the thruster.
//        // Strafing or example will temporarily take the ship out of engine kill.
//        if (requestedEngineKill && !ship.ShipControls.thruster && Mathf.Abs(ship.ShipControls.horizontalStrafe) < 0.1f)
//        {
//            linearDrag = 1.0f;
//            engineState = EngineState.Killed;
//        }

//        // Main engine and strafe works only when not in engine kill.
//        else
//        {
//            linearDrag = startingLinearDrag;

//            // If last frame was an enginekill frame, restore the engine state to running.
//            if (engineStateLastFrame == EngineState.Killed)
//                engineState = EngineState.Running;

//            // Use the full range of motion for the throttle.
//            float trueThrottle = Mathf.Clamp(throttle, -reverseFraction, 1.0f);

//            // Use either cruise thrust or normal thrust.
//            float trueForwardForce = forwardForce;
//            if (engineState == EngineState.Cruising)
//                trueForwardForce = CruiseRamp();

//            ship.Rigidbody.AddRelativeForce(ship.ShipControls.horizontalStrafe * strafeForce,
//                                0.0f,
//                                (trueForwardForce * trueThrottle));

//            // Thruster works during engine kill, but not during cruise.
//            if (ship.ShipControls.thruster
//                && (engineState != EngineState.Cruising && engineState != EngineState.Charging)
//                && thrustPower > thrustCapacity * 0.05f)
//            {
//                thrustPower -= thrustDrain * Time.deltaTime;
//                thrustPower = Mathf.Max(thrustPower, 0.0f);
//                ship.Rigidbody.AddRelativeForce(Vector3.forward * thrusterForce);
//            }
//        }

//        // Rotation.
//        ship.Rigidbody.AddRelativeTorque(ship.ShipControls.pitch * steeringTorque.x * MULTIPLIER,
//                                         ship.ShipControls.yaw * steeringTorque.y * MULTIPLIER,
//                                         //Mathf.Clamp(-ship.ShipControls.roll + ship.ShipControls.yaw * -0.5f, -1.0f, 1.0f) * steeringTorque.z * MULTIPLIER);
//                                         -ship.ShipControls.roll * steeringTorque.z * MULTIPLIER);

//        // Calculate own drag using Freelancer's equation.
//        ship.Rigidbody.AddForce(-linearDrag * ship.Rigidbody.velocity);

//        // Angular drag.
//        Vector3 angDragForce = transform.InverseTransformDirection(ship.Rigidbody.angularVelocity);
//        angDragForce.x *= -angularDrag.x;
//        angDragForce.y *= -angularDrag.y;
//        angDragForce.z *= -angularDrag.z;
//        ship.Rigidbody.AddRelativeTorque(angDragForce);

//        speed = ship.Rigidbody.velocity.magnitude;
//    }
//}
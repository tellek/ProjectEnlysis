using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerControl : MonoBehaviour {

    public GameObject PlayerObject;
    public float SpinSpeed = 30;
    public GameObject[] Cores;
    public GameObject[] Engines;
    public GameObject[] Thrusters;

    private bool isBuilding = false;

    private GameObject core;
    private int coreNum = 0;
    private GameObject engine;
    private int engNum = 0;
    private GameObject leftThruster;
    private int ltNum = 0;
    private GameObject rightThruster;
    private int rtNum = 0;
	
	void Update () {
        if (Input.GetKeyDown("c"))
        {
            if (!isBuilding)
            {
                isBuilding = true;
                StartProcess();
            }
            else if (isBuilding)
            {
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;

                // Get the current player ship.
                foreach (Transform child in PlayerObject.transform)
                {
                    pos = child.position;
                    rot = child.rotation;
                    Destroy(child.gameObject);
                }

                // Replace the current ship with the newly built one.
                Instantiate(core, pos, rot, PlayerObject.transform);

                // Build room deactivated.
                DestroyAllParts();
                isBuilding = false;
            }
        }
        if (isBuilding)
        {
            // Constant ship rotation in the builder.
            transform.Rotate((Vector3.up * Time.deltaTime) * SpinSpeed);

            // Change the core
            if (Input.GetKeyDown("1"))
            {
                if (coreNum >= Cores.Length - 1) coreNum = 0;
                else coreNum++;
                SetNewCore();
            }
            // Change the engine part
            if (Input.GetKeyDown("2"))
            {
                SetPart(ref engine, Engines, ref engNum, "Engine");
            }
            // Change the left thruster part
            if (Input.GetKeyDown("3"))
            {
                SetPart(ref leftThruster, Thrusters, ref ltNum, "LeftThruster");
            }
            // Change the right thruster part
            if (Input.GetKeyDown("4"))
            {
                SetPart(ref rightThruster, Thrusters, ref rtNum, "RightThruster");
            }
        }
    }

    private void SetPart(ref GameObject part, GameObject[] theList, ref int theCount, string partName)
    {
        // Get the array id to be used
        if (theCount >= theList.Length - 1) theCount = 0;
        else theCount++;

        // Get positions and rotations
        var pos = part.transform.position;
        var rot = part.transform.rotation;

        Destroy(part);

        // Ready the part
        part = theList[theCount];

        // Find the mount point
        var mount = core.transform.Find(partName);

        // Create the part
        part = Instantiate(part, pos, rot, mount);
    }

    private void StartProcess()
    {
        ReadyParts();

        // Create the Core
        core = Instantiate(core, transform.position, Quaternion.identity, transform);

        // Create the Engine
        var engineMount = core.transform.Find("Engine");
        engine = Instantiate(engine, engineMount.transform.position, engine.transform.rotation, engineMount);

        // Create the left thruster
        var ltMount = core.transform.Find("LeftThruster");
        leftThruster = Instantiate(leftThruster, ltMount.transform.position, leftThruster.transform.rotation, ltMount);

        // Create the right thruster
        var rtMount = core.transform.Find("RightThruster");
        rightThruster = Instantiate(rightThruster, rtMount.transform.position, rightThruster.transform.rotation, rtMount);
    }

    // Since all parts are attached to the core, everything needs to be rebuilt if the core is changed.
    private void SetNewCore()
    {
        // Get positions and rotations of all parts.
        var pos = core.transform.position;
        var rot = core.transform.rotation;
        var engPos = engine.transform.position;
        var engRot = engine.transform.rotation;
        var ltPos = leftThruster.transform.position;
        var ltRot = leftThruster.transform.rotation;
        var rtPos = rightThruster.transform.position;
        var rtRot = rightThruster.transform.rotation;

        // Destroying the core also destroys the other attached parts.
        Destroy(core);

        ReadyParts();

        // Create the Core
        core = Instantiate(core, pos, rot, transform);

        // Create the Engine
        var engineMount = core.transform.Find("Engine");
        engine = Instantiate(engine, engPos, engRot, engineMount);

        // Create the left thruster
        var ltMount = core.transform.Find("LeftThruster");
        leftThruster = Instantiate(leftThruster, ltPos, ltRot, ltMount);

        // Create the right thruster
        var rtMount = core.transform.Find("RightThruster");
        rightThruster = Instantiate(rightThruster, rtPos, rtRot, rtMount);
    }

    private void DestroyAllParts()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ReadyParts()
    {
        // Ready parts using existing array counts (This will need to be different when arrays can change based on what parts you have.)
        core = Cores[coreNum];
        engine = Engines[engNum];
        leftThruster = Thrusters[ltNum];
        rightThruster = Thrusters[rtNum];
    }

}

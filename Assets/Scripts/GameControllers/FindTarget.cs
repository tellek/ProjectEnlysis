using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour {

    public float TargetRange = 150.0f;
    public float RotateToTargetSpeed = 3f;
    public bool haveTarget = false;
    public GameObject target;

    private string currentTarget = "none";
    private Dictionary<int, GameObject> results = new Dictionary<int, GameObject>();
    private Dictionary<int, float> targetDistances = new Dictionary<int, float>();
    private SpriteRenderer sRenderer;
    private List<int> previousTargets = new List<int>();

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
		if (Input.GetKeyDown("t")) 
        {
            if (haveTarget)
            {
                RemoveCurrentTarget();
                previousTargets.Clear();
            }
            else
            {
                LoopThroughAvailableTargets();
            }
        }

        if (Input.GetKeyDown("n"))
        {
            LoopThroughAvailableTargets();
        }

        if (haveTarget)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > TargetRange)
            {
                RemoveCurrentTarget();
            }
        }
        
	}

    private void FixedUpdate()
    {
        if (haveTarget)
        {
            //rb.freezeRotation = true;
            var toRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * RotateToTargetSpeed);
        }
        //else rb.freezeRotation = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 30, Screen.width, Screen.height), "Target: " + currentTarget);
    }

    private void LoopThroughAvailableTargets()
    {
        PopulateInRangeResults();
        if (targetDistances.Count <= 0) return;
        int newTarget = GetClosestTarget(targetDistances);

        if (previousTargets.Contains(newTarget))
        {
            Dictionary<int, float> tempDistances = new Dictionary<int, float>();
            foreach (var tg in targetDistances)
            {
                if (!previousTargets.Contains(tg.Key)) tempDistances.Add(tg.Key, tg.Value);
            }

            if (tempDistances.Count <= 0) previousTargets.Clear();
            else newTarget = GetClosestTarget(tempDistances);
        }
        SetTarget(newTarget);
        previousTargets.Add(newTarget);
    }

    private void PopulateInRangeResults()
    {
        GameObject[] allResults = GameObject.FindGameObjectsWithTag("Enemy");

        results.Clear();
        targetDistances.Clear();

        foreach (var ar in allResults)
        {
            int id = ar.GetInstanceID();
            float distance = Vector3.Distance(transform.position, ar.transform.position);

            if (distance <= TargetRange)
            {
                results.Add(id, ar);
                targetDistances.Add(id, distance);
            }
        }
    }

    private int GetClosestTarget(Dictionary<int, float> distances)
    {
        float distance = float.MaxValue;
        int resultingId = 0;

        foreach (var td in distances)
        {
            if (td.Value < distance)
            {
                distance = td.Value;
                resultingId = td.Key;
            }
        }
        return resultingId;
    }

    private void SetTarget(int targetId)
    {
        RemoveCurrentTarget();

        target = results[targetId];
        var marker = target.transform.Find("TargetMarker");
        sRenderer = marker.GetComponent<SpriteRenderer>();
        sRenderer.enabled = true;
        haveTarget = true;

        currentTarget = "[" + targetId + "] " + target.name;
    }

    private void RemoveCurrentTarget()
    {
        if (haveTarget) sRenderer.enabled = false;
        haveTarget = false;
        currentTarget = "none";
    }
}

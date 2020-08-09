using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceTest : MonoBehaviour
{
    public List<GameObject> Sensors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = new Ray(transform.position, -transform.up);

        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //	Vector3 reflectVec = hit.normal;

        //	Debug.DrawLine(this.transform.position, hit.point, Color.red);
        //	Debug.DrawRay(hit.point, reflectVec, Color.green, 100);
        //}

        ConeRayCast(8, this.transform.position, 100);
        
    }

    private void ConeRayCast(int rayCount, Vector3 emitPoint, float maxDistance)
    {
        foreach (var sensor in Sensors)
        {
            Vector3 dir = (sensor.transform.position - transform.position).normalized;
            var r1 = new Ray(transform.position, dir);
            Debug.DrawLine(r1.origin, r1.GetPoint(10), Color.red);

            RaycastHit hit;
            if (Physics.Raycast(r1, out hit))
            {
                Vector3 reflectVec = hit.normal;
                Debug.DrawRay(hit.point, reflectVec, Color.green);
            }
        }



        //var straightRay = new Ray(transform.position, -transform.up);
        //var farPoint = straightRay.GetPoint(10);
        //Debug.DrawLine(straightRay.origin, farPoint, Color.cyan);

        //Vector3 dir = (transform.position - transform.position).normalized;
        //var r1 = new Ray(transform.position, dir);

        ////var r1 = new Ray(transform.position, -transform.up);
        //Debug.DrawLine(r1.origin, r1.GetPoint(10), Color.red);
        //RaycastHit hit;
        //if (Physics.Raycast(r1, out hit))
        //{
        //    Vector3 reflectVec = hit.normal;
        //    Debug.DrawRay(hit.point, reflectVec, Color.green);
        //}
        


        //for (float i = 0; i < maxDegree; i += increment)
        //{
        //    var ray = new Ray(transform.position, -transform.up);

        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Vector3 reflectVec = hit.normal;

        //        Debug.DrawLine(this.transform.position, hit.point, Color.red);
        //        Debug.DrawRay(hit.point, reflectVec, Color.green);
        //    }
        //}
    }
}

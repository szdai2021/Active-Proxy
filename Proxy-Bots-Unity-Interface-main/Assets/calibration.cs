using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class calibration : MonoBehaviour
{
    public GameObject surface;

    public GameObject topLeft_tracked;
    public GameObject topLeft;
    public GameObject bottomRight_tracked;
    public GameObject bottomRight;

    public GameObject robot01;
    public GameObject navMeshParent;

    public bool mapVirtualTable = false;
    //public bool bakeNavMesh = false;
    public bool robotCalibration = false;

    // Update is called once per frame
    void Update()
    {
        if (mapVirtualTable)
        {
            // surface.transform.position += bottomRight_tracked.transform.position - bottomRight.transform.position;

            //navMeshParent.transform.position += bottomRight_tracked.transform.position - surface.transform.position;
            surface.transform.position = bottomRight_tracked.transform.position;
            
            // surface.transform.LookAt(topLeft_tracked.transform, topLeft.transform.position - bottomRight.transform.position);

            // surface.transform.LookAt(topLeft_tracked.transform, Vector3.up);

            surface.transform.localEulerAngles = new Vector3(0, Vector3.SignedAngle(topLeft.transform.position - bottomRight.transform.position, topLeft_tracked.transform.position - bottomRight_tracked.transform.position, Vector3.up), 0);
            //navMeshParent.transform.localEulerAngles = surface.transform.localEulerAngles;

            mapVirtualTable = false;
        }

        /*
        if (bakeNavMesh)
        {

            foreach (Transform t in navMeshParent.transform)
            {
                t.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
            }

            bakeNavMesh = false;
        }
        */

        if (robotCalibration)
        {

            robotCalibration = false;
        }
    }
}

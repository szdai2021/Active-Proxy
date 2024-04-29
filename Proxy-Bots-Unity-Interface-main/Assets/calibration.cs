using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calibration : MonoBehaviour
{
    public GameObject surface;

    public GameObject topLeft_tracked;
    public GameObject topLeft;
    public GameObject bottomRight_tracked;
    public GameObject bottomRight;

    public GameObject robot01;

    public bool mapVirtualTable = false;
    public bool robotCalibration = false;

    // Update is called once per frame
    void Update()
    {
        if (mapVirtualTable)
        {
            // surface.transform.position += bottomRight_tracked.transform.position - bottomRight.transform.position;

            surface.transform.position = bottomRight_tracked.transform.position;

            // surface.transform.LookAt(topLeft_tracked.transform, topLeft.transform.position - bottomRight.transform.position);

            // surface.transform.LookAt(topLeft_tracked.transform, Vector3.up);

            surface.transform.localEulerAngles = new Vector3(0, Vector3.SignedAngle(topLeft.transform.position - bottomRight.transform.position, topLeft_tracked.transform.position - bottomRight_tracked.transform.position, Vector3.up), 0);

            mapVirtualTable = false;
        }

        if (robotCalibration)
        {

            robotCalibration = false;
        }
    }
}

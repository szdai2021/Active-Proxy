using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class informationSync : MonoBehaviour
{
    public ConnectionManager cm;

    public GameObject robotAgent;
    public bool Start = false;

    // Update is called once per frame
    void Update()
    {
        if (Start)
        {
            // send robot position to sever
            for(int i = 0; i < 6; i ++)
            {
                Transform t = robotAgent.transform.GetChild(i);

                float orientation = Vector3.SignedAngle(t.GetChild(1).position - t.GetChild(0).position, Vector3.forward, Vector3.up);

                cm.Send(
                    "R" + i.ToString() + " " + 
                    "(" + t.position.x + "," + t.position.z + ")" + " " +
                    orientation.ToString("F4")); // rotation later
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class interactionManager : MonoBehaviour
{
    public GameObject map;
    public GameObject mapPivot;
    public GameObject[] agents;
    public GameObject[] destinations;
    public GameObject cylinder;
    public ConnectionManager connectionManager;

    public List<bool> pickUpDetection = new List<bool>();
    private List<bool> prevPickUpDetection = new List<bool>();
    public Collider pickUpDetectionArea;

    public executePath executePath;

    private Vector3 mapPos;
    private Vector3 mapScale;
    private Vector3 mapRot;

    private List<Vector3> agentPos = new List<Vector3>();

    private int totalPickUp = 0;
    private Vector3 sumMovement;
    private float scaleChange;
    private float mapY;

    public bool testFlag1;
    public bool testFlag2;
    public bool testFlag3;

    public GameObject maptemp;

    private float tempValue = -1;
    private Vector3 tempVector = new Vector3(0, 0, 0);

    private void Start()
    {
        mapPos = map.transform.position;
        mapRot = map.transform.rotation.eulerAngles;
        mapScale = map.transform.localScale;

        mapY = map.transform.localPosition.y;

        prevPickUpDetection = pickUpDetection;
    }

    // Update is called once per frame
    void Update()
    {
        //connectionManager.Send("REF_RBH");
        totalPickUp = pickUpDetection.Count(cc => cc == true);

        
        // picking up detection
        for (int i = 0; i<6; i++)
        {
            // robot is picked up if it's been moved higher than certain level
            if (pickUpDetectionArea.bounds.Contains(agents[i].transform.position))
            {
                pickUpDetection[i] = true;
                executePath.agentPickedUp[i] = true;
            }
            else
            {
                pickUpDetection[i] = false;
                executePath.agentPickedUp[i] = false;
            }
        }
        

        if (totalPickUp == 2)
        {
            // get the index of picked bot
            int a = -1;
            int b = -1;

            for (int i = 0; i < 6; i++)
            {
                if (pickUpDetection[i])
                {
                    if (a == -1)
                    {
                        a = i;
                    }
                    else
                    {
                        b = i;
                    }
                }
            }

            if (testFlag1)
            {
                // position update
                sumMovement = (agents[a].transform.position - agentPos[a] + agents[b].transform.position - agentPos[b]) / 2;

                map.transform.position += sumMovement;
            }

            if (testFlag2)
            {
                // scale update
                scaleChange = Vector3.Distance(agents[a].transform.position, agents[b].transform.position) / Vector3.Distance(agentPos[a], agentPos[b]);

                map.transform.localScale *= scaleChange;
            }

            if (testFlag3)
            {
                if (tempVector.Equals(new Vector3(0, 0, 0)))
                {
                    tempVector = agents[a].transform.position - agents[b].transform.position;

                    cylinder.transform.position = new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z);
                    cylinder.transform.LookAt(new Vector3(agents[b].transform.position.x, cylinder.transform.position.y, agents[b].transform.position.z));
                }
                else
                {
                    RaycastHit hit;
                    Vector3 intersectionPoint = Vector3.zero;

                    if (Physics.Raycast(new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z), (agents[a].transform.position - agents[b].transform.position), out hit, 10f))
                    {
                        intersectionPoint = hit.point;
                    }
                    else if(Physics.Raycast(new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z), (- agents[a].transform.position + agents[b].transform.position), out hit, 10f))
                    {
                        intersectionPoint = hit.point;
                    }

                    if (!intersectionPoint.Equals(Vector3.zero))
                    {
                        float angleDifference = Vector3.SignedAngle(tempVector, agents[a].transform.position - agents[b].transform.position, Vector3.up);

                        map.transform.RotateAround(intersectionPoint, Vector3.up, angleDifference);

                        //map.transform.eulerAngles = new Vector3(map.transform.eulerAngles.x, map.transform.eulerAngles.y, map.transform.eulerAngles.z + maptemp.transform.localEulerAngles.y - tempVector.y);
                        
                        cylinder.transform.position = new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z);
                        cylinder.transform.LookAt(new Vector3(agents[b].transform.position.x, cylinder.transform.position.y, agents[b].transform.position.z));

                        tempVector = agents[a].transform.position - agents[b].transform.position;
                    }
                }
            }

            map.transform.localPosition = new Vector3(map.transform.localPosition.x, mapY ,map.transform.localPosition.z);
        }
        else
        {
            tempValue = -1;
            tempVector = new Vector3(0, 0, 0);
        }

        // update agent pos
        for (int i = 0; i<6; i++)
        {
            agentPos.Insert(i, agents[i].transform.position);
        }

        // send message to server
        for (int i = 0; i < 6; i++)
        {
            if (prevPickUpDetection[i] != pickUpDetection[i])
            {
                if (pickUpDetection[i])
                {
                    //connectionManager.Send("Agent " + i.ToString() + " has been picked up.");
                    connectionManager.Send("REF_PAC");
                }
                else
                {
                    //connectionManager.Send("Agent " + i.ToString() + " has been dropped down.");
                    connectionManager.Send("REF_RBH");
                }
            }
        }

        prevPickUpDetection = pickUpDetection;
    }
}

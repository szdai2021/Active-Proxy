using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class interactionManager : MonoBehaviour
{
    public GameObject map;
    public GameObject[] agents;
    public GameObject[] destinations;
    public GameObject cylinder;

    public List<bool> pickUpDetection = new List<bool>();
    private List<bool> prevPickUpDetection = new List<bool>();
    public Collider pickUpDetectionArea;

    public executePath executePath;

    public GameObject mapTemp1;

    public GameObject mapY_Reference;

    private Vector3 mapPos;
    private Vector3 mapScale;
    private Quaternion mapRot;

    private List<Vector3> agentPos = new List<Vector3>();

    private int totalPickUp = 0;
    private Vector3 sumMovement;
    private float scaleChange;
    private float mapY;

    public bool posControl;
    public bool scaleControl;
    public bool rotControl;

    private float tempValue = -1;
    private Vector3 tempVector = new Vector3(0, 0, 0);
    public GameObject centerMarker;

    private void Start()
    {
        mapPos = map.transform.position;
        mapRot = map.transform.rotation;
        mapScale = map.transform.localScale;

        mapY = mapY_Reference.transform.position.y;

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

        // update map
        updateMap();

        // update agent pos
        for (int i = 0; i<6; i++)
        {
            agentPos.Insert(i, agents[i].transform.position);
        }

        // reset map
        if (Input.GetKeyDown("r"))
        {
            map.transform.position = mapPos;
            map.transform.rotation = mapRot;
            map.transform.localScale = mapScale;
        }

        prevPickUpDetection = pickUpDetection;
    }

    private void updateMap()
    {
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

            if (posControl)
            {
                // position update
                sumMovement = (agents[a].transform.position - agentPos[a] + agents[b].transform.position - agentPos[b]) / 2;

                //map.transform.position += sumMovement;

                map.transform.position += sumMovement;
            }

            if (scaleControl)
            {
                // use temp map to set correct map pos and then update the map
                // setup temp1
                mapTemp1.transform.position = (agents[a].transform.position + agents[b].transform.position)/2;
                mapTemp1.transform.rotation = map.transform.rotation;
                mapTemp1.transform.localScale = map.transform.localScale;

                map.transform.GetChild(0).SetParent(mapTemp1.transform);

                // scale update
                scaleChange = Vector3.Distance(agents[a].transform.position, agents[b].transform.position) / Vector3.Distance(agentPos[a], agentPos[b]);

                //map.transform.localScale *= scaleChange;

                mapTemp1.transform.localScale *= scaleChange;

                mapTemp1.transform.GetChild(0).SetParent(map.transform);
            }

            if (rotControl)
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

                    if (Physics.Raycast(new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z), (agents[a].transform.position - agents[b].transform.position).normalized, out hit, 10f))
                    {
                        intersectionPoint = hit.point;
                    }
                    else if (Physics.Raycast(new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z), (-agents[a].transform.position + agents[b].transform.position).normalized, out hit, 10f))
                    {
                        intersectionPoint = hit.point;
                    }
                    else if (Vector3.Distance(agents[a].transform.position, agentPos[a]) < 0.001)
                    {
                        intersectionPoint = agents[a].transform.position;
                    }
                    else if (Vector3.Distance(agents[b].transform.position, agentPos[b]) < 0.001)
                    {
                        intersectionPoint = agents[b].transform.position;
                    }

                    intersectionPoint = new Vector3(intersectionPoint.x, mapY, intersectionPoint.z);
                    centerMarker.transform.position = intersectionPoint;

                    /*
                    // use temp map to set correct map pos and then update the map
                    // setup temp1
                    mapTemp1.transform.position = intersectionPoint;
                    mapTemp1.transform.rotation = map.transform.rotation;
                    mapTemp1.transform.localScale = map.transform.localScale;

                    map.transform.GetChild(0).SetParent(mapTemp1.transform);

                    if (!intersectionPoint.Equals(new Vector3(0, mapY, 0)))
                    {
                        float angleDifference = Vector3.SignedAngle(tempVector, agents[a].transform.position - agents[b].transform.position, Vector3.up);

                        //map.transform.RotateAround(intersectionPoint, Vector3.up, angleDifference);

                        mapTemp1.transform.Rotate(new Vector3(0,angleDifference,0), Space.Self);

                        // update clyinder position
                        cylinder.transform.position = new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z);
                        cylinder.transform.LookAt(new Vector3(agents[b].transform.position.x, cylinder.transform.position.y, agents[b].transform.position.z));

                        tempVector = agents[a].transform.position - agents[b].transform.position;
                    }
                    
                    mapTemp1.transform.GetChild(0).SetParent(map.transform);
                    */

                    float angleDifference = Vector3.SignedAngle(tempVector, agents[a].transform.position - agents[b].transform.position, Vector3.up);

                    updateMapRotation(intersectionPoint, angleDifference);

                    // update clyinder position
                    cylinder.transform.position = new Vector3(agents[a].transform.position.x, cylinder.transform.position.y, agents[a].transform.position.z);
                    cylinder.transform.LookAt(new Vector3(agents[b].transform.position.x, cylinder.transform.position.y, agents[b].transform.position.z));

                    tempVector = agents[a].transform.position - agents[b].transform.position;

                }
                
            }

            if (rotControl & false)
            {
                Vector2 p1 = new Vector2(agents[a].transform.position.x, agents[a].transform.position.z);
                Vector2 p1_prev = new Vector2(agentPos[a].x, agentPos[a].z);

                Vector2 p2 = new Vector2(agents[b].transform.position.x, agents[b].transform.position.z);
                Vector2 p2_prev = new Vector2(agentPos[b].x, agentPos[b].z);

                Vector2 p = FindIntersection(p1, p2, p1_prev, p2_prev);

                float angleDifference = Vector3.SignedAngle(tempVector, agents[a].transform.position - agents[b].transform.position, Vector3.up);

                updateMapRotation(new Vector3(p.x, mapY, p.y), angleDifference);

                centerMarker.transform.position = new Vector3(p.x, mapY, p.y);
            }

            map.transform.position = new Vector3(map.transform.position.x, mapY, map.transform.position.z);
        }
    }

    private void updateMapRotation(Vector3 intersectionPoint, float angleChange)
    {
        GameObject mapTempChild = mapTemp1.transform.GetChild(0).gameObject;

        mapTemp1.transform.position = intersectionPoint;
        mapTemp1.transform.rotation = map.transform.rotation;
        mapTemp1.transform.localScale = map.transform.localScale;

        mapTempChild.transform.position = map.transform.GetChild(0).position;
        mapTempChild.transform.rotation = map.transform.GetChild(0).rotation;
        mapTempChild.transform.localScale = map.transform.GetChild(0).localScale;

        mapTemp1.transform.Rotate(new Vector3(0f, angleChange, 0f), Space.Self);

        map.transform.GetChild(0).position = mapTempChild.transform.position;
        map.transform.GetChild(0).rotation = mapTempChild.transform.rotation;
        map.transform.GetChild(0).localScale = mapTempChild.transform.localScale;
    }

    public static Vector2 FindIntersection(Vector2 current1, Vector2 current2, Vector2 prev1, Vector2 prev2)
    {
        // Direction vectors of the lines
        Vector2 direction1 = current2 - current1;
        Vector2 direction2 = prev2 - prev1;

        // Check if lines are parallel
        float det = direction1.x * direction2.y - direction1.y * direction2.x;

        // Parameters for the intersection point
        float t = (direction1.x * (prev1.y - current1.y) + direction1.y * (current1.x - prev1.x)) / det;

        // Calculate intersection point
        float intersectX = prev1.x + t * direction2.x;
        float intersectY = prev1.y + t * direction2.y;

        return new Vector2(intersectX, intersectY);
    }
}

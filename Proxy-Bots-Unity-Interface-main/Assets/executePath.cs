using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class executePath : MonoBehaviour
{
    public GameObject[] agents;

    public RobotControl robotControl;
    public GameObject navMeshParent;
    public GameObject robotAgentParent;

    public Dictionary<int, List<Vector3>> pathDic = new Dictionary<int, List<Vector3>>(6);

    public List<Vector3> currentDestination = new List<Vector3>(new Vector3[6]);

    [Header("--- Robot Configuration ---", order = 1)]
    [Space(-10, order = 2)]
    [Header("The configuration settings of the robot", order = 3)]

    public float arrivingThreshold = 0.03f; // unit: cm
    public float turningThreshold = 5f; // unit: degree

    public int rotationSpeed = 100;
    public int movingSpeed = 100;

    public bool start = false;

    private List<bool> rotationFinished = new List<bool>(new bool[6]);

    public bool bakeNavMesh = false;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            rotateRobotByPath();
            moveRobotByPath();
        }

        if (bakeNavMesh)
        {
            foreach (Transform t in navMeshParent.transform)
            {
                t.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
            }

            foreach (Transform t in robotAgentParent.transform)
            {
                t.gameObject.GetComponent<robotDriver>().hasPath = false;
            }

            bakeNavMesh = false;
        }
    }

    private void rotateRobotByPath()
    {
        // calculate turning angle and execute
        for (int i = 0; i < 6; i++)
        {
            NavMeshPath path = agents[i].GetComponent<NavMeshAgent>().path;

            if (path.corners.Length > 1)
            {
                Vector3 headingDirection = agents[i].transform.GetChild(1).position - agents[i].transform.GetChild(0).position;
                Vector3 turningDirection = path.corners[1] - path.corners[0];

                float turningAngle = Vector3.SignedAngle(headingDirection, turningDirection, Vector3.up);

                print(i + " " + turningAngle);

                // execute robot turning command if angle difference is bigger than threshold
                if (!rotationFinished[i])
                {
                    if (Mathf.Abs(turningAngle) > turningThreshold)
                    {
                        /*
                        if (turningAngle < 0)
                        {
                            robotControl.RobotMove(3, rotationSpeed); // index 3: left turn
                        } 
                        else
                        {
                            robotControl.RobotMove(4, rotationSpeed + 50); // index 4: right turn
                        }
                        */
                        robotControl.RobotMove(3, rotationSpeed); // index 3: left turn

                        print(i + " " + "rotating");
                    }
                    else
                    {
                        rotationFinished[i] = true;
                        robotControl.Stop();

                        print(i + " " + "stopping rotation");
                    }
                }
                else
                {
                    if (turningAngle > turningThreshold)
                    {
                        rotationFinished[i] = false;
                    }
                }
            }
        }

    }

    private void moveRobotByPath()
    {
        
        // calculate distance and execute
        for (int i = 0; i < 6; i++)
        {
            NavMeshPath path = agents[i].GetComponent<NavMeshAgent>().path;

            if (path.corners.Length > 1)
            {
                //print(i + " " + Vector3.Distance(agents[i].transform.position, path.corners[1]));

                // only execute when the heading direction is correct
                if (rotationFinished[i])
                {
                    // execute robot turning command if angle difference is bigger than threshold
                    if (Vector3.Distance(agents[i].transform.position, path.corners[1]) < arrivingThreshold)
                    {
                        robotControl.Stop();
                        //print(i + " " + "stopping moving forward");
                    }
                    else
                    {
                        robotControl.RobotMove(1, movingSpeed); // index 1: move forward
                        //print(i + " " + "moving forward");
                    }
                }
            }
        }
    }

    private void setCurrentDestinationIndex()
    {
        for (int i = 0; i < 6; i++)
        {

        }
    }
}

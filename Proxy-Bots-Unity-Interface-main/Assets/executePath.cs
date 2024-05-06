using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

public class executePath : MonoBehaviour
{
    public GameObject[] agents;

    public RobotControl robotControl;
    public GameObject navMeshParent;

    public GameObject stateInfoBoard;

    public Dictionary<int, List<Vector3>> pathDic = new Dictionary<int, List<Vector3>>(6);

    [Header("--- Robot Configuration ---", order = 1)]
    [Space(-10, order = 2)]
    [Header("The configuration settings of the robot", order = 3)]

    public float arrivingThreshold = 0.03f; // unit: cm
    public float turningThreshold = 5f; // unit: degree

    public int rotationSpeedL = 100;
    public int rotationSpeedR = 100;
    public int movingSpeedL = 100;
    public int movingSpeedR = 100;

    public bool start = false;

    private List<bool> rotationFinished = new List<bool>(new bool[6]);
    public List<bool> destinationFinished = new List<bool>(new bool[6]);

    private Dictionary<int, int> robotStageMachineState = new Dictionary<int, int>(6);

    public bool bakeNavMesh = false;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            rotateRobotByPath();
            moveRobotByPath();

            stopRobotAfterArriving();
        }

        

        if (bakeNavMesh)
        {
            foreach (Transform t in navMeshParent.transform)
            {
                t.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
            }

            bakeNavMesh = false;
        }

    }

    private void stopRobotAfterArriving()
    {
        for (int i = 0; i < 6; i++)
        {
            string robotName = "/bot" + (i + 1).ToString();
            if (destinationFinished[i])
            {
                robotControl.Stop(robotName);
            }
        }
    }

    private void rotateRobotByPath()
    {
        // calculate turning angle and execute
        for (int i = 0; i < 6; i++)
        {
            NavMeshPath path = agents[i].GetComponent<NavMeshAgent>().path;
            string robotName = "/bot" + (i + 1).ToString();

            if (path.corners.Length > 1 & !destinationFinished[i])
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
                        int rsL, rsR;

                        if (Mathf.Abs(turningAngle) < 45)
                        {
                            rsL = rotationSpeedL / 2;
                            rsR = rotationSpeedR / 2;
                        }
                        else
                        {
                            rsL = rotationSpeedL;
                            rsR = rotationSpeedR;
                        }

                        if (turningAngle < 0)
                        {
                            robotControl.RobotMove(1, rsL, rsR, robotName); // index 1: left turn
                        } 
                        else
                        {
                            robotControl.RobotMove(2, rsL, rsR, robotName); // index 2: right turn
                        }

                        stateInfoBoard.transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = "Rotating";
                    }
                    else
                    {
                        rotationFinished[i] = true;
                        robotControl.Stop(robotName);

                        stateInfoBoard.transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = "Stopping Rotation";
                    }
                }
                else
                {
                    if (Mathf.Abs(turningAngle) > turningThreshold)
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
            string robotName = "/bot" + (i + 1).ToString();

            if (path.corners.Length > 1 & !destinationFinished[i])
            {
                print(i + " " + Vector3.Distance(agents[i].transform.position, path.corners[path.corners.Length - 1]));

                // only execute when the heading direction is correct
                if (rotationFinished[i])
                {
                    // execute robot turning command if angle difference is bigger than threshold
                    if (Vector3.Distance(agents[i].transform.position, path.corners[path.corners.Length-1]) < arrivingThreshold)
                    {
                        robotControl.Stop(robotName);

                        stateInfoBoard.transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = "Stopping Moving";

                        destinationFinished[i] = true;
                    }
                    else
                    {
                        if (Vector3.Distance(agents[i].transform.position, path.corners[path.corners.Length - 1]) < arrivingThreshold * 8)
                        {
                            robotControl.RobotMove(3, movingSpeedL/2, movingSpeedR/2, robotName); // index 3: move forward
                        }
                        else
                        {
                            robotControl.RobotMove(3, movingSpeedL, movingSpeedR, robotName); // index 3: move forward
                        }

                        stateInfoBoard.transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = "Moving Forward";

                        destinationFinished[i] = false;
                    }
                }
            }
        }
    }
}

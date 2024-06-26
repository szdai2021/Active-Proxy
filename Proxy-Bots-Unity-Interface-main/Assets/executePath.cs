﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

public class executePath : MonoBehaviour
{
    public GameObject[] agents;
    public GameObject[] destinations;

    public RobotControl robotControl;
    public GameObject navMeshParent;

    public GameObject stateInfoBoard;

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
    public List<bool> orientationFinished = new List<bool>(new bool[6]);
    [HideInInspector]
    public List<bool> agentPickedUp = new List<bool>(new bool[6]);

    private List<Vector3> path = new List<Vector3>(new Vector3[6]);

    public bool orientationCheck = false;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!agentPickedUp[i])
                {
                    path = new List<Vector3>(new Vector3[6]);

                    foreach (Vector3 v in agents[i].GetComponent<NavMeshAgent>().path.corners)
                    {
                        path.Insert(path.Count - 1, v);
                    }

                    rotateRobotByPath(path, i);
                    moveRobotByPath(path, i);

                    stopRobotAfterArriving(i);

                    /*
                    if (destinationFinished[1])
                    {
                        if (!orientationFinished[i])
                        {
                            path = new List<Vector3>();

                            path.Insert(0, destinations[i].transform.GetChild(0).position);
                            path.Insert(0, destinations[i].transform.position);

                            orientationMatch(path, i);
                        }

                    }
                    */
                }
            }
        }

    }

    private void stopRobotAfterArriving(int i)
    {
        string robotName = "/bot" + (i + 1).ToString();
        if (destinationFinished[i])
        {
            robotControl.Stop(robotName);
        }
    }

    private void rotateRobotByPath(List<Vector3> plannedPath, int i)
    {
        string robotName = "/bot" + (i + 1).ToString();

        if (plannedPath.Count > 1 & !destinationFinished[i])
            {
                Vector3 headingDirection = agents[i].transform.GetChild(1).position - agents[i].transform.GetChild(0).position;
                Vector3 turningDirection = plannedPath[1] - plannedPath[0];

                float turningAngle = Vector3.SignedAngle(headingDirection, turningDirection, Vector3.up);

                // print(i + " " + turningAngle);

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


    private void orientationMatch(List<Vector3> plannedPath, int i)
    {
        string robotName = "/bot" + (i + 1).ToString();

        if (plannedPath.Count > 1 & destinationFinished[i])
        {
            Vector3 headingDirection = agents[i].transform.GetChild(1).position - agents[i].transform.GetChild(0).position;
            Vector3 turningDirection = plannedPath[1] - plannedPath[0];

            float turningAngle = Vector3.SignedAngle(headingDirection, turningDirection, Vector3.up);

            // execute robot turning command if angle difference is bigger than threshold
            if (destinationFinished[i])
            {
                if (Mathf.Abs(turningAngle) > 5)
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
                }
                else
                {
                    orientationFinished[i] = true;
                    robotControl.Stop(robotName);
                }
            }
        }
    }


    private void moveRobotByPath(List<Vector3> plannedPath, int i)
    {
            string robotName = "/bot" + (i + 1).ToString();

            if (plannedPath.Count > 1 & !destinationFinished[i])
            {
                // print(i + " " + Vector3.Distance(agents[i].transform.position, path.corners[path.corners.Length - 1]));

                // only execute when the heading direction is correct
                if (rotationFinished[i])
                {
                    // execute robot turning command if angle difference is bigger than threshold
                    if (Vector3.Distance(agents[i].transform.position, plannedPath[plannedPath.Count - 1]) < arrivingThreshold)
                    {
                        robotControl.Stop(robotName);

                        stateInfoBoard.transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = "Stopping Moving";

                        destinationFinished[i] = true;
                    }
                    else
                    {
                        // check if the heading direction is still correct
                        Vector3 headingDirection = agents[i].transform.GetChild(1).position - agents[i].transform.GetChild(0).position;
                        Vector3 turningDirection = plannedPath[1] - plannedPath[0];

                        float turningAngle = Vector3.SignedAngle(headingDirection, turningDirection, Vector3.up);

                        int leftOffset;
                        int rightOffset;

                        adjustMoveSpeed(out leftOffset, out rightOffset, turningAngle, i);

                        if (Vector3.Distance(agents[i].transform.position, plannedPath[plannedPath.Count - 1]) < arrivingThreshold * 8)
                        {
                            robotControl.RobotMove(3, (movingSpeedL + leftOffset)/2, (movingSpeedR + rightOffset)/2, robotName); // index 3: move forward
                        }
                        else
                        {
                            robotControl.RobotMove(3, movingSpeedL + leftOffset, movingSpeedR + rightOffset, robotName); // index 3: move forward
                        }

                        stateInfoBoard.transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = "Moving Forward";

                        destinationFinished[i] = false;
                    }
                }
            }
    }

    private void adjustMoveSpeed(out int leftOffset, out int rightOffset, float turningAngle, int i)
    {
        leftOffset = 0;
        rightOffset = 0;

        if (Mathf.Abs(turningAngle) > 5)
        {
            if (turningAngle < 0)
            {
                rightOffset = 5 * ((int)Mathf.Abs(turningAngle) / 5 + 1);
                leftOffset = 0;
            }
            else
            {
                rightOffset = 0;
                leftOffset = 5 * ((int)Mathf.Abs(turningAngle) / 5 + 1);
            }
        }
        else
        {
            leftOffset = 0;
            rightOffset = 0;
        }
    }

}

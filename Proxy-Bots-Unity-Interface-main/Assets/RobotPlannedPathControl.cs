using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlannedPathControl : MonoBehaviour
{
    public GameObject[] agents;
    public RobotControl robotControl;

    public GameObject initialPosParent;
    public GameObject pathPos1;

    public GameObject destinationParent;

    public PlannedPathControl plannedPathControl;

    public bool toInitialPosFlag = false;
    public bool startLineUpMoveFlag = false;
    public bool startCircularMoveFlag = false;

    private List<bool> rotationFinished = new List<bool>(new bool[6]);
    public List<bool> destinationFinished = new List<bool>(new bool[6]);

    public float turningThreshold = 10;
    public int rotationSpeedL = 50;
    public int rotationSpeedR = 50;
    public int movingSpeedL = 200;
    public int movingSpeedR = 200;
    public float arrivingThreshold = 0.04f;

    public List<Vector3> p1 = new List<Vector3>();
    public List<Vector3> p2 = new List<Vector3>();
    public List<Vector3> p3 = new List<Vector3>();
    public List<Vector3> p4 = new List<Vector3>();
    public List<Vector3> p5 = new List<Vector3>();
    public List<Vector3> p6 = new List<Vector3>();

    private List<int> adjustCounter = new List<int>(6);

    // Update is called once per frame
    void Update()
    { 
        if (toInitialPosFlag)
        {
            toInitialPos();

            toInitialPosFlag = false;
        }

        if (startLineUpMoveFlag)
        {
            startLineUpMove();
        }

        if (startCircularMoveFlag)
        {
            startCircularMove();
        }
    }

    private void toInitialPos()
    {
        for (int i = 0; i < 6; i++)
        {
            destinationParent.transform.GetChild(i).transform.position = initialPosParent.transform.GetChild(i).transform.position;
        }
    }

    private void startLineUpMove()
    {
        rotateRobotByPath();
        moveRobotByPath();
        stopRobotAfterArriving();
    }

    private void startCircularMove()
    {
        for (int i = 0; i < 6; i++)
        {
            rotationFinished[i] = true;
        }
        moveRobotByPath();
        stopRobotAfterArriving();
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
            List<Vector3> currentPath = plannedPathControl.pathByRobotIndex[i];
            updateCurrentDestination(i, currentPath);
            string robotName = "/bot" + (i + 1).ToString();

            if (currentPath.Count > 1 & !destinationFinished[i])
            {
                Vector3 headingDirection = agents[i].transform.GetChild(1).position - agents[i].transform.GetChild(0).position;
                Vector3 turningDirection = currentPath[1] - currentPath[0];

                float turningAngle = Vector3.SignedAngle(headingDirection, turningDirection, Vector3.up);

                if (Vector3.Distance(agents[i].transform.position, currentPath[currentPath.Count - 1]) < 0.1)
                {
                    rotationFinished[i] = true;
                }

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
                    }
                    else
                    {
                        rotationFinished[i] = true;
                        robotControl.Stop(robotName);
                    }
                }
                else
                {
                    if (Mathf.Abs(turningAngle) > turningThreshold & Vector3.Distance(agents[i].transform.position, currentPath[currentPath.Count - 1]) > 0.1)
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
            List<Vector3> currentPath = plannedPathControl.pathByRobotIndex[i];
            updateCurrentDestination(i, currentPath);
            string robotName = "/bot" + (i + 1).ToString();

            if (currentPath.Count > 1 & !destinationFinished[i])
            {
                // only execute when the heading direction is correct
                if (rotationFinished[i])
                {
                    // execute robot turning command if angle difference is bigger than threshold
                    if (Vector3.Distance(agents[i].transform.position, currentPath[currentPath.Count - 1]) < arrivingThreshold)
                    {
                        robotControl.Stop(robotName);

                        destinationFinished[i] = true;
                    }
                    else
                    {
                        // check if the heading direction is still correct
                        Vector3 headingDirection = agents[i].transform.GetChild(1).position - agents[i].transform.GetChild(0).position;
                        Vector3 turningDirection = currentPath[1] - currentPath[0];

                        float turningAngle = Vector3.SignedAngle(headingDirection, turningDirection, Vector3.up);

                        int leftOffset;
                        int rightOffset;

                        adjustMoveSpeed(out leftOffset, out rightOffset, turningAngle, i);

                        if (Vector3.Distance(agents[i].transform.position, currentPath[currentPath.Count - 1]) < arrivingThreshold * 8)
                        {
                            robotControl.RobotMove(3, (movingSpeedL + leftOffset) / 2, (movingSpeedR + rightOffset) / 2, robotName); // index 3: move forward
                        }
                        else
                        {
                            robotControl.RobotMove(3, movingSpeedL + leftOffset, movingSpeedR + rightOffset, robotName); // index 3: move forward
                        }

                        destinationFinished[i] = false;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(agents[i].transform.position, currentPath[currentPath.Count - 1]) > arrivingThreshold*2)
                {
                    destinationFinished[i] = false;
                }
            }
        }
    }

    private void updateCurrentDestination(int i, List<Vector3> list)
    {
        switch (i)
        {
            case 0:
                p1 = new List<Vector3>();

                foreach (Vector3 v in list)
                {
                    p1.Add(v);
                }
                break;
            case 1:
                p2 = new List<Vector3>();

                foreach (Vector3 v in list)
                {
                    p2.Add(v);
                }
                break;
            case 2:
                p3 = new List<Vector3>();

                foreach (Vector3 v in list)
                {
                    p3.Add(v);
                }
                break;
            case 3:
                p4 = new List<Vector3>();

                foreach (Vector3 v in list)
                {
                    p4.Add(v);
                }
                break;
            case 4:
                p5 = new List<Vector3>();

                foreach (Vector3 v in list)
                {
                    p5.Add(v);
                }
                break;
            case 5:
                p6 = new List<Vector3>();

                foreach (Vector3 v in list)
                {
                    p6.Add(v);
                }
                break;
            default:
                break;
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
                rightOffset = 5 * ((int)Mathf.Abs(turningAngle)/5 + 1);
                leftOffset = 0;
            }
            else
            {
                rightOffset = 0;
                leftOffset = 5 * ((int)Mathf.Abs(turningAngle)/5 + 1);
            }
        }
        else
        {
            leftOffset = 0;
            rightOffset = 0;
        }
    }
}


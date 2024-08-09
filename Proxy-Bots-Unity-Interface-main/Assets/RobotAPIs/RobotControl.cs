﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;  //https://github.com/Iam1337/extOSC



public class RobotControl : MonoBehaviour
{

    public OSCTransmitter transmitter;
    public string robotName = "/bot1";
    public int testSpeedL=100;
    public int testSpeedR = 100;
    public bool enableKeystrokeTest;

    public List<int> speedLOffset = new List<int>(6);
    public List<int> speedROffset = new List<int>(6);

    public GameObject robotParent;
    public GameObject colliderParent;

    public bool[] robotEnable;
    public List<bool> robotStopped = new List<bool> { true, true, true, true, true, true };

    private List<Vector3> robotPosPre = new List<Vector3> { Vector3.zero, Vector3.zero , Vector3.zero, Vector3.zero , Vector3.zero , Vector3.zero };
    private bool delayFlag = false;
    private List<bool> delayRobotStopped = new List<bool> { true, true, true, true, true, true };

    private IEnumerator delayTimer()
    {
        while (true)
        {
            if (delayFlag)
            {
                yield return new WaitForSeconds(1f);
                robotStopped = delayRobotStopped;
                delayFlag = false;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Start()
    {
        StartCoroutine(delayTimer());
    }

    void Update()
    {
        if (enableKeystrokeTest)
        {

            if (Input.GetKeyDown(KeyCode.W))
            {
                RobotMove(1, testSpeedL, testSpeedR);               

            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RobotMove(2, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                RobotMove(3, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                RobotMove(4, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                Stop();
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (robotPosPre[i] != null & Vector3.Distance(robotPosPre[i], robotParent.transform.GetChild(i).position) > 0.01)
            {
                robotStopped[i] = false;
            }

            robotPosPre[i] = robotParent.transform.GetChild(i).position;
        }
    }

    public void RobotMove(int mode, int speedL, int speedR, string name = null)  // Mode: 1 left, 2 right, 3 forward, 4 revers.  Speed 0-255, 20 seems to be about the slowest.
    {
        int i = 0;
        int spL = speedL;
        int spR = speedR;

        if (name != null)
        {
            robotName = name;

            // find the index of the robot
            i = int.Parse(name[name.Length - 1].ToString());
            //print(i);
        }
        else
        {
            name = robotName;
            i = int.Parse(robotName[robotName.Length - 1].ToString());
        }

        if (mode == 3)
        {
            switch (name)
            {
                case "/bot1":
                    spL += speedLOffset[0];
                    spR += speedROffset[0];
                    break;
                case "/bot2":
                    spL += speedLOffset[1];
                    spR += speedROffset[1];
                    break;
                case "/bot3":
                    spL += speedLOffset[2];
                    spR += speedROffset[2];
                    break;
                case "/bot4":
                    spL += speedLOffset[3];
                    spR += speedROffset[3];
                    break;
                case "/bot5":
                    spL += speedLOffset[4];
                    spR += speedROffset[4];
                    break;
                case "/bot6":
                    spL += speedLOffset[5];
                    spR += speedROffset[5];
                    break;
                default:
                    break;
            }
        }

        foreach(Transform t in colliderParent.transform)
        {
            if (t.gameObject.GetComponent<BoxCollider>().bounds.Contains(robotParent.transform.GetChild(i-1).transform.position))
            {
                print("robot" + i + " is too close to the edge");

                if (robotStopped[i-1])
                {
                    return;
                }
                else
                {
                    Stop(name);
                    return;
                }
            }
        }

        if (robotEnable[i-1])
        {
            var message = new OSCMessage(robotName);
            message.AddValue(OSCValue.Int(mode));
            message.AddValue(OSCValue.Int(spL));
            message.AddValue(OSCValue.Int(spR));
            transmitter.Send(message);

            robotStopped[i-1] = false;
            delayRobotStopped[i - 1] = false;
        }
    }

    public void Move(int speedL, int speedR)
    {
        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(1));
        message.AddValue(OSCValue.Int(speedL));
        message.AddValue(OSCValue.Int(speedR));
        transmitter.Send(message);
    }

    public void Rotate(int speedL, int speedR)
    {
        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(4));
        message.AddValue(OSCValue.Int(speedL));
        message.AddValue(OSCValue.Int(speedR));
        transmitter.Send(message);
    }

    public void Stop(string name = null)
    {
        if (name == null)
        {
            name = robotName;
        }

        int i = int.Parse(name[name.Length - 1].ToString());

        var message = new OSCMessage(name);
        message.AddValue(OSCValue.Int(0)); 
        message.AddValue(OSCValue.Int(0));
        message.AddValue(OSCValue.Int(0));
        transmitter.Send(message);

        delayFlag = true;
        delayRobotStopped[i - 1] = true;
    }
}

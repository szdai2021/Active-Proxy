using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;  //https://github.com/Iam1337/extOSC



public class robotControlAll : MonoBehaviour
{

    public OSCTransmitter transmitter;
    public int testSpeedL = 100;
    public int testSpeedR = 100;
    public bool enableKeystrokeTest;

    private List<string> robotList = new List<string>(6);
    
    private void Start()
    {
        robotList.Add("/bot1");
        robotList.Add("/bot2");
        robotList.Add("/bot3");
        robotList.Add("/bot4");
        robotList.Add("/bot5");
        robotList.Add("/bot6");
    }

    void Update()
    {
        if (enableKeystrokeTest)
        {

            if (Input.GetKeyDown(KeyCode.W))
            {
                RobotMove(3, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RobotMove(4, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                RobotMove(1, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                RobotMove(2, testSpeedL, testSpeedR);

            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                Stop();
            }
        }
    }

    public void RobotMove(int mode, int speedL, int speedR, string name = null)  // Mode: 1 left, 2 right, 3 forward, 4 revers.  Speed 0-255, 20 seems to be about the slowest.
    {
        foreach (string n in robotList)
        {
            var message = new OSCMessage(n);
            message.AddValue(OSCValue.Int(mode));
            message.AddValue(OSCValue.Int(speedL));
            message.AddValue(OSCValue.Int(speedR));
            transmitter.Send(message);
        }
    }

    public void Stop(string name = null)
    {
        foreach (string n in robotList)
        {
            var message = new OSCMessage(n);
            message.AddValue(OSCValue.Int(0));
            message.AddValue(OSCValue.Int(0));
            message.AddValue(OSCValue.Int(0));
            transmitter.Send(message);
        }
    }
}

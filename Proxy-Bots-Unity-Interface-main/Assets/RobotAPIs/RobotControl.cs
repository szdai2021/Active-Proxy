using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;  //https://github.com/Iam1337/extOSC



public class RobotControl : MonoBehaviour
{

    public OSCTransmitter transmitter;
    public string robotName = "/Robot1";
    public int testSpeedL=100;
    public int testSpeedR = 100;
    public bool enableKeystrokeTest;

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
        if (name != null)
        {
            robotName = name;
        }

        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(mode));
        message.AddValue(OSCValue.Int(speedL)); 
        message.AddValue(OSCValue.Int(speedR));
        transmitter.Send(message);
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
        var message = new OSCMessage(name);
        message.AddValue(OSCValue.Int(0)); 
        message.AddValue(OSCValue.Int(0));
        message.AddValue(OSCValue.Int(0));
        transmitter.Send(message);
    }
}

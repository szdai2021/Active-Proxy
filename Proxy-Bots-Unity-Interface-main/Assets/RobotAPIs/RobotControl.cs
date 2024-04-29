using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;  //https://github.com/Iam1337/extOSC



public class RobotControl : MonoBehaviour
{

    public OSCTransmitter transmitter;
    public string robotName = "/Robot1";
    public int testSpeed=100;
    public bool enableKeystrokeTest;

    void Update()
    {
        if (enableKeystrokeTest)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                RobotMove(1, testSpeed);               

            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RobotMove(2, testSpeed);

            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                RobotMove(3, testSpeed);

            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                Stop();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                RobotMove(4, testSpeed);

            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                Stop();
            }
        }
    }

    public void RobotMove(int mode, int speed, string name = null)  // Mode: 1 forward, 2 revers, 3 left, 4 right.  Speed 0-255, 20 seems to be about the slowest.
    {
        if (name != null)
        {
            robotName = name;
        }

        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(mode));
        message.AddValue(OSCValue.Int(speed));                                              
        transmitter.Send(message);
    }

    public void Move(int speed)
    {       
        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(1));
        message.AddValue(OSCValue.Int(speed));
        transmitter.Send(message);
    }

    public void Rotate(int speed)
    {
        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(4));
        message.AddValue(OSCValue.Int(speed));
        transmitter.Send(message);
    }

    public void Stop()
    {
        var message = new OSCMessage(robotName);
        message.AddValue(OSCValue.Int(0)); 
        message.AddValue(OSCValue.Int(0));                                  
        transmitter.Send(message);
    }
}

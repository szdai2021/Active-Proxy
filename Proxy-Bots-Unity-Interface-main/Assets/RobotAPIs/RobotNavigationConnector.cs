using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNavigationConnector : MonoBehaviour
{
    public RobotControl robotControl;
    public VirtualRobotNavigation virtualRobot;

    void Awake()
    {
        virtualRobot.OnMove.AddListener(OnVirtualMove);
        virtualRobot.OnRotate.AddListener(OnVirtualRotate);
        virtualRobot.OnStop.AddListener(OnVirtualStop);
    }

    private void OnVirtualStop()
    {
        robotControl.Stop();
    }

    private void OnVirtualMove(float speed)
    {
        int robotSpeed = VirtualSpeedToActualSpeed(speed);
        Debug.Log("Moving at " + robotSpeed);
        robotControl.Move(VirtualSpeedToActualSpeed(robotSpeed));
    }

    private void OnVirtualRotate(float angle)
    {
        int robotSpeed = VirtualAngleToActualSpeed(angle);
        Debug.Log("Rotating at " + robotSpeed);

        robotControl.Rotate(robotSpeed);
    }

    private int VirtualSpeedToActualSpeed(float speed)
    {
        return (int) (speed * 500);
    }

    private int VirtualAngleToActualSpeed(float angle)
    {
        return (int) (angle * 50);
    }
}

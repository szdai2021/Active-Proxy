using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualRobotNavigationTest : MonoBehaviour
{
    public Transform target;
    public VirtualRobotNavigation virtualRobot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            virtualRobot.MoveToward(target.position, delegate { });
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            virtualRobot.RotateToward(target.position, delegate { });
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Go();
        }
    }

    public void Go()
    {
        virtualRobot.Navigate(target.position, Vector3.forward);
    }
}

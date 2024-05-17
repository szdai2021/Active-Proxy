using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlannedPathControl : MonoBehaviour
{
    public RobotControl robotControl;

    public GameObject initialPosParent;
    public GameObject pathPos1;

    public GameObject destinationParent;

    public bool toInitialPosFlag = false;
    public bool startLineUpMoveFlag = false;
    public bool startCircularMoveFlag = false;


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
        for (int i = 0; i < 6; i++)
        {
            destinationParent.transform.GetChild(i).transform.position = pathPos1.transform.GetChild(i).transform.position;
        }
    }

    private void startCircularMove()
    {

    }
}


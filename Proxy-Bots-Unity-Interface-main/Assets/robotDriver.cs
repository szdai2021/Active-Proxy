using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class robotDriver : MonoBehaviour
{
    public GameObject destination;
    public NavMeshSurface surface;
    public GameObject robotClone;

    public TextMeshPro t;

    public bool stopRobot = false;

    // Update is called once per frame
    void Update()
    {
        if (stopRobot)
        {
            this.GetComponent<NavMeshAgent>().speed = 0;
            this.GetComponent<NavMeshAgent>().angularSpeed = 0;
            this.GetComponent<NavMeshAgent>().acceleration = 0;
        }
        else
        {
            this.GetComponent<NavMeshAgent>().speed = 0.5f;
            this.GetComponent<NavMeshAgent>().angularSpeed = 120f;
            this.GetComponent<NavMeshAgent>().acceleration = 0.3f;
        }

        surface.BuildNavMesh();
        this.GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);

        robotClone.transform.position = this.transform.position;

        NavMeshPath path = null;
        this.GetComponent<NavMeshAgent>().CalculatePath(destination.transform.position, path);

        DrawPath(path);
    }

    void DrawPath(NavMeshPath path)
    {

        print(path.corners.Length);

        this.GetComponent<LineRenderer>().positionCount = path.corners.Length; //set the array of positions to the amount of corners

        for (int i = 1; i < path.corners.Length; i++)
        {
            this.GetComponent<LineRenderer>().SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class robotDriver : MonoBehaviour
{
    public GameObject destination;
    public NavMeshSurface surface;

    public TextMeshPro t;

    public executePath executePath;

    public bool stopRobot = false;
    public bool hasPath = false;

    private Vector3 prevDestination;

    // Update is called once per frame
    void Update()
    {
        if (prevDestination != destination.transform.position)
        {
            hasPath = false;

            string name = this.gameObject.name;
            int i = int.Parse(name[name.Length-1].ToString());

            //print(i);

            executePath.destinationFinished[i-1] = false;
        }

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

        if (!hasPath)
        {
            findPath();
        }
        else
        {
            drawPath();

            string s = "";

            foreach (Vector3 a in this.GetComponent<NavMeshAgent>().path.corners)
            {
                s += a.ToString() + " ";
            }

            t.text = s;
        }

        prevDestination = destination.transform.position;
    }

    public void findPath()
    {
        surface.BuildNavMesh();
        this.GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);

        hasPath = this.GetComponent<NavMeshAgent>().hasPath;
    }

    private void drawPath()
    {
        this.GetComponent<LineRenderer>().positionCount = this.GetComponent<NavMeshAgent>().path.corners.Length;

        this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);

        if (this.GetComponent<NavMeshAgent>().path.corners.Length < 2)
        {
            return;
        }

        for (int i = 1; i < this.GetComponent<NavMeshAgent>().path.corners.Length; i++)
        {
            this.GetComponent<LineRenderer>().SetPosition(i, this.GetComponent<NavMeshAgent>().path.corners[i]);
        }
    }
}

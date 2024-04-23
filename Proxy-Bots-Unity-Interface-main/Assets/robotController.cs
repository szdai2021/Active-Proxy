using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class robotController : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent[] agents;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                foreach (NavMeshAgent a in agents)
                {
                    a.SetDestination(hit.point);
                }
            }
        }
    }
}

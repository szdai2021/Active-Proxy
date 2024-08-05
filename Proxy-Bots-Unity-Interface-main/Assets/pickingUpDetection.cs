using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class pickingUpDetection : MonoBehaviour
{
    public GameObject[] agents;
    public GameObject[] destinations;

    public List<bool> pickUpDetection = new List<bool>(new bool[6]);
    private List<bool> prevPickUpDetection = new List<bool>(new bool[6]);
    public Collider pickUpDetectionArea;

    public executePath executePath;

    private List<Vector3> agentPos = new List<Vector3>();

    private int totalPickUp = 0;

    // Update is called once per frame
    void Update()
    {
        totalPickUp = pickUpDetection.Count(cc => cc == true);

        // picking up detection
        for (int i = 0; i < 6; i++)
        {
            // robot is picked up if it's been moved higher than certain level
            if (pickUpDetectionArea.bounds.Contains(agents[i].transform.position))
            {
                pickUpDetection[i] = true;
                executePath.agentPickedUp[i] = true;
            }
            else
            {
                pickUpDetection[i] = false;
                executePath.agentPickedUp[i] = false;

                if (prevPickUpDetection[i])
                {
                    executePath.destinationFinished[i] = false;
                    executePath.orientationFinished[i] = false;
                }
            }

            prevPickUpDetection[i] = pickUpDetection[i];
        }

        // update agent pos
        for (int i = 0; i < 6; i++)
        {
            agentPos.Insert(i, agents[i].transform.position);
        }

    }
}

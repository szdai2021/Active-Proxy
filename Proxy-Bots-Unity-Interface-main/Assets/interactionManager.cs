using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionManager : MonoBehaviour
{
    public GameObject map;
    public GameObject[] agents;
    public GameObject[] destinations;

    public List<bool> pickUpDetection = new List<bool>();
    public Collider pickUpDetectionArea;

    public executePath executePath;

    private Vector3 mapPos;
    private Vector3 mapScale;
    private Vector3 mapRot;

    private List<Vector3> agentPos = new List<Vector3>();

    private void Start()
    {
        mapPos = map.transform.position;
        mapRot = map.transform.rotation.eulerAngles;
        mapScale = map.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // picking up detection
        for (int i = 0; i<6; i++)
        {
            // robot is picked up if it's been moved higher than certain level
            if (pickUpDetectionArea.bounds.Contains(agents[i].transform.position))
            {
                pickUpDetection[i] = true;
            }
            else
            {
                pickUpDetection[i] = false;
            }
        }

        // update agent pos
        for (int i = 0; i<6; i++)
        {
            agentPos.Insert(i, agents[i].transform.position);
        }
    }
}

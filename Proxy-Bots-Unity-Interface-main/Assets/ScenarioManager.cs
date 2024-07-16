using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public ConnectionManager cm;

    public GameObject robotAgents;
    public GameObject destinations;

    public GameObject robotClones;
    public GameObject destinationClones;

    public float tempValue = 0;

    private Dictionary<int, Vector2> destinationReceived = new Dictionary<int, Vector2>();
    private Dictionary<int, float> rotationReceived = new Dictionary<int, float>();


    // Update is called once per frame
    void Update()
    {
        destinationReceived = cm.posReceived;
        rotationReceived = cm.rotReceived;

        for (int i = 0; i < 6; i ++)
        {
            destinationClones.transform.GetChild(i).position = new Vector3(destinationReceived[i].x, tempValue, destinationReceived[i].y);
            destinationClones.transform.GetChild(i).rotation = Quaternion.Euler(0, rotationReceived[i], 0);
        }
    }

    // convert destination to unity coordinate; convert robot to table display coordinate
    private void transformationMatrix(GameObject g1_U, GameObject g2_U, GameObject g1_T, GameObject g2_T)
    {
        float distance_U = Vector3.Distance(g1_U.transform.position, g2_U.transform.position);
        float distance_T = Vector3.Distance(g1_T.transform.position, g2_T.transform.position);

        float angle_U = Vector3.SignedAngle(g1_U.transform.forward, g2_U.transform.forward, Vector3.up);
        float angle_T = Vector3.SignedAngle(g2_U.transform.forward, g2_U.transform.forward, Vector3.up);

        float scaleDifference = distance_U / distance_T;
        float orientationDifference = angle_U - angle_T;

        Vector3 translation_1 = g1_U.transform.position - g1_T.transform.position;
        Vector3 translation_2 = g2_U.transform.position - g2_T.transform.position;
    }
}

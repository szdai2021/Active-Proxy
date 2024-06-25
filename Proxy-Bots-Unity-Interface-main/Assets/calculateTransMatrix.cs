using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calculateTransMatrix : MonoBehaviour
{
    [HideInInspector]
    public Matrix4x4 transMatrix = Matrix4x4.zero;
    private Matrix4x4 unityCoordMatrix = Matrix4x4.zero;
    private Matrix4x4 robotCoordMatrix = Matrix4x4.zero;

    public GameObject p1;
    public GameObject p1_twin;
    public GameObject p2;
    public GameObject p2_twin;
    public GameObject p3;
    public GameObject p3_twin;
    public GameObject p4;
    public GameObject p4_twin;

    public bool Calculate = false;

    // Update is called once per frame
    void Update()
    {

        if (Calculate)
        {
            transMatrix = Matrix4x4.zero;
            unityCoordMatrix = Matrix4x4.zero;
            robotCoordMatrix = Matrix4x4.zero;

            robotCoordMatrix[0, 0] = p1.transform.position.x;
            robotCoordMatrix[1, 0] = p1.transform.position.y;
            robotCoordMatrix[2, 0] = p1.transform.position.z;
            robotCoordMatrix[3, 0] = 1f;

            unityCoordMatrix[0, 0] = p1_twin.transform.position.x;
            unityCoordMatrix[1, 0] = p1_twin.transform.position.y;
            unityCoordMatrix[2, 0] = p1_twin.transform.position.z;
            unityCoordMatrix[3, 0] = 1f;



            robotCoordMatrix[0, 1] = p2.transform.position.x;
            robotCoordMatrix[1, 1] = p2.transform.position.y;
            robotCoordMatrix[2, 1] = p2.transform.position.z;
            robotCoordMatrix[3, 1] = 1f;

            unityCoordMatrix[0, 1] = p2_twin.transform.position.x;
            unityCoordMatrix[1, 1] = p2_twin.transform.position.y;
            unityCoordMatrix[2, 1] = p2_twin.transform.position.z;
            unityCoordMatrix[3, 1] = 1f;



            robotCoordMatrix[0, 2] = p3.transform.position.x;
            robotCoordMatrix[1, 2] = p3.transform.position.y;
            robotCoordMatrix[2, 2] = p3.transform.position.z;
            robotCoordMatrix[3, 2] = 1f;

            unityCoordMatrix[0, 2] = p3_twin.transform.position.x;
            unityCoordMatrix[1, 2] = p3_twin.transform.position.y;
            unityCoordMatrix[2, 2] = p3_twin.transform.position.z;
            unityCoordMatrix[3, 2] = 1f;



            robotCoordMatrix[0, 3] = p4.transform.position.x;
            robotCoordMatrix[1, 3] = p4.transform.position.y;
            robotCoordMatrix[2, 3] = p4.transform.position.z;
            robotCoordMatrix[3, 3] = 1f;

            unityCoordMatrix[0, 3] = p4_twin.transform.position.x;
            unityCoordMatrix[1, 3] = p4_twin.transform.position.y;
            unityCoordMatrix[2, 3] = p4_twin.transform.position.z;
            unityCoordMatrix[3, 3] = 1f;

            transMatrix = robotCoordMatrix * unityCoordMatrix.inverse;
            //transMatrix = unityCoordMatrix * robotCoordMatrix.inverse;

            Calculate = false;
        }
    }
}

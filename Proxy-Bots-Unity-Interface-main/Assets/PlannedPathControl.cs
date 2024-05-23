using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannedPathControl : MonoBehaviour
{
    enum pathOptionType { linear, circular}
    [SerializeField] pathOptionType pathOption;

    public GameObject linearPathParent;
    public GameObject circularPathParent;

    public GameObject robotParent;

    [HideInInspector]
    public Dictionary<int, List<Vector3>> pathByRobotIndex = new Dictionary<int, List<Vector3>>();
    
    // Update is called once per frame
    void Update()
    {
        switch (pathOption)
        {
            case (pathOptionType)0:
                circularPathParent.SetActive(false);
                linearPathParent.SetActive(true);

                pathByRobotIndex = new Dictionary<int, List<Vector3>>();

                for (int i = 0; i < 6; i++)
                {
                    List<Vector3> p = new List<Vector3>();
                    p.Insert(0, linearPathParent.transform.GetChild(i).position);
                    p.Insert(0, robotParent.transform.GetChild(i).position);

                    pathByRobotIndex.Add(i, p);
                }
                break;
            case (pathOptionType)1:
                linearPathParent.SetActive(false);
                circularPathParent.SetActive(true);

                pathByRobotIndex = new Dictionary<int, List<Vector3>>();

                for (int i = 0; i < 6; i++)
                {
                    List<Vector3> p = new List<Vector3>();
                    p.Insert(0, circularPathParent.transform.GetChild(i).position);
                    p.Insert(0, robotParent.transform.GetChild(i).position);

                    pathByRobotIndex.Add(i, p);
                }
                break;
            default:
                break;
        }
    }
}

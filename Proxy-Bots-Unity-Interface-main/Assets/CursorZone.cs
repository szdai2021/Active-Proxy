using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CursorZone : MonoBehaviour
{
    public int zoneId;
    public GameObject[] colliderList;

    public OnEnterCursorZone onCursorEnter = new OnEnterCursorZone();
    public OnExitCursorZone onCursorExit = new OnExitCursorZone();
    public onDetectBookMarkAction onDetect = new onDetectBookMarkAction();

    public bool triggerEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("cursor"))
        {
            triggerEnter = true;
            onCursorEnter.Invoke(zoneId, t.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("cursor"))
        {
            triggerEnter = false;
            onCursorExit.Invoke(zoneId, t.name);
        }
    }
}

[System.Serializable]
public class OnEnterCursorZone : UnityEvent<int, string>
{
}

public class OnExitCursorZone : UnityEvent<int, string>
{

}

public class onDetectBookMarkAction : UnityEvent<int, string>
{

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookMarkActionDetection : MonoBehaviour
{
    public GameObject[] cursorZoneList;

    public OnActionDone onActionEnter = new OnActionDone();
    public OnActionUndo onActionExit = new OnActionUndo();

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("bookmark"))
        {
            foreach (GameObject g in cursorZoneList)
            {
                if (g.GetComponent<CursorZone>().triggerEnter)
                {
                    onActionEnter.Invoke(g.GetComponent<CursorZone>().zoneId, t.parent.name);
                }
            }
        }
    }
}

[System.Serializable]
public class OnActionDone : UnityEvent<int, string>
{
}

public class OnActionUndo : UnityEvent<int, string>
{

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorZoneDataTransmitter : MonoBehaviour
{

    public APIManager apiManager;
    public CursorZone zone;

    private void Awake()
    {
        zone.onCursorEnter.AddListener(SendEnter);
        zone.onCursorExit.AddListener(SendExit);
        zone.onDetect.AddListener(SendBookMark);
    }

    private void SendExit(int id, string refName)
    {
        apiManager.SendHoverExit(id, refName);
    }

    private void SendEnter(int id, string refName)
    {
        apiManager.SendHoverEnter(id, refName);
    }

    private void SendBookMark(int id, string refName)
    {
        apiManager.SendBookMark(id, refName);
    }

}

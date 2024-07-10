using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDataTransmitter : MonoBehaviour
{

    public APIManager apiManager;
    public Zone zone;

    private void Awake()
    {
        zone.onEnter.AddListener(SendEnter);
        zone.onTracking.AddListener(SendTracking);
        zone.onExit.AddListener(SendExit);
        zone.onDwelling.AddListener(SendDwelling);
        zone.onDwelled.AddListener(SendDwelled);
    }

    private void SendDwelling(int id, string refName, float normTime)
    {
        apiManager.SendDwelling(id, refName, normTime); 
    }

    private void SendDwelled(int id, string refName)
    {
        apiManager.SendDwelled(id, refName);
    }
    private void SendExit(int id, string refName)
    {
        apiManager.SendExitZone(id, refName);
    }

    private void SendTracking(int id, float d, string refName)
    {
        apiManager.SendTrackedDistance(id, d, refName);
    }

    private void SendEnter(int id, string refName)
    {
        apiManager.SendEnterZone(id, refName);
    }
}

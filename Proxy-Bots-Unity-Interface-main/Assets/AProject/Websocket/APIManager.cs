using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class APIManager : MonoBehaviour
{
    public ConnectionManager connectionManager;

    public bool connectOnStart = false;

    [Header("Enable actions")]
    public bool sendPickupEvent;
    public bool sendPlacedEvent;
    public bool sendAirDwelledEvent;
    public bool sendZoneEnterEvent;
    public bool sendZoneExitEvent;
    public bool sendZoneTrackingDistanceEvent;
    public bool sendZoneDwelledEvent;
    public bool sendZoneDwellingTimeEvent;
    public bool sendPitchForwardEvent;
    public bool sendPitchBackwardEvent;
    public bool sendPitchExitEvent;
    public bool sendRollRightEvent;
    public bool sendRollLeftEvent;
    public bool sendRollExitEvent;

    public bool sendHoverEnterEvent;
    public bool sendHoverExitEvent;
    public bool sendBookMarkEvent;

    private const string ACTION_PICKUP = "pickup";
    private const string ACTION_PLACED = "placed";
    private const string ACTION_AIRDWELLED = "airdwelled";
    private const string ACTION_ZONE_ENTER = "zoneenter";
    private const string ACTION_ZONE_EXIT_ZONE = "zoneexit";
    private const string ACTION_ZONE_TRACKING_DISTANCE = "zonetracking";
    private const string ACTION_ZONE_DWELLED = "zonedwelled";
    private const string ACTION_ZONE_DWELLING_TIME = "zonedwellingtime";
    private const string ACTION_PITCH_FORWARD = "pitchforward";
    private const string ACTION_PITCH_BACKWARD = "pitchbackward";
    private const string ACTION_PITCH_EXIT = "pitchexit";
    private const string ACTION_ROLL_RIGHT = "rollright";
    private const string ACTION_ROLL_LEFT = "rollleft";
    private const string ACTION_ROLL_EXIT = "rollexit";

    private const string ACTION_HOVER_ENTER = "hoverenter";
    private const string ACTION_HOVER_EXIT = "hoverexit";
    private const string ACTION_BOOK_MARK = "bookmarkenter";


    private void Start()
    {
        if(connectOnStart) connectionManager.Connect();
    }

    public void SendPickUp(Transform t)
    {
       if(sendPickupEvent)  connectionManager.Send(ACTION_PICKUP + "-" + t.name);
    }

    public void SendPlaced(Transform t)
    {
        if (sendPlacedEvent) connectionManager.Send(ACTION_PLACED + "-" + t.name);
    }

    public void SendAirDwelled(Transform t)
    {
        if (sendAirDwelledEvent) connectionManager.Send(ACTION_AIRDWELLED + "-" + t.name);
    }

    public void SendTrackedDistance(int id, float d, string refName)
    {
        if (sendZoneTrackingDistanceEvent) connectionManager.Send(ACTION_ZONE_TRACKING_DISTANCE + "-" + id + "-" + d + "-" + refName);
    }

    public void SendExitZone(int id, string refName)
    {
        if (sendZoneExitEvent) connectionManager.Send(ACTION_ZONE_EXIT_ZONE + "-" + id + "-" + refName);
    }

    public void SendDwelling(int id, string refName, float normTime)
    {
        if (sendZoneDwellingTimeEvent) connectionManager.Send(ACTION_ZONE_DWELLING_TIME + "-" + id + "-" + refName + "-" + normTime);
    }

    public void SendDwelled(int id, string refName)
    {
        if (sendZoneDwelledEvent) connectionManager.Send(ACTION_ZONE_DWELLED + "-" + id + "-" + refName);
    }

    public void SendEnterZone(int id, string refName)
    {
        if (sendZoneEnterEvent) connectionManager.Send(ACTION_ZONE_ENTER + "-" + id + "-" + refName);
    }

    public void SendPitchForward(Transform t)
    {
        if (sendPitchForwardEvent) connectionManager.Send(ACTION_PITCH_FORWARD + "-" + t.name);
    }

    public void SendPitchBackward(Transform t)
    {
        if (sendPitchBackwardEvent) connectionManager.Send(ACTION_PITCH_BACKWARD + "-" + t.name);
    }

    public void SendPitchExit(Transform t)
    {
        if (sendPitchExitEvent) connectionManager.Send(ACTION_PITCH_EXIT+ "-" + t.name);
    }

    public void SendRollRight(Transform t)
    {
        if (sendRollRightEvent) connectionManager.Send(ACTION_ROLL_RIGHT + "-" + t.name);
    }

    public void SendRollLeft(Transform t)
    {
        if (sendRollLeftEvent) connectionManager.Send(ACTION_ROLL_LEFT+ "-" + t.name);
    }

    public void SendRollExit(Transform t)
    {
        if (sendRollExitEvent) connectionManager.Send(ACTION_ROLL_EXIT + "-" + t.name);
    }

    public void SendHoverEnter(int id, string refName)
    {
        if (sendHoverEnterEvent) connectionManager.Send(ACTION_HOVER_ENTER + "-" + id + "-" + refName);
    }

    public void SendHoverExit(int id, string refName)
    {
        if (sendHoverExitEvent) connectionManager.Send(ACTION_HOVER_EXIT + "-" + id + "-" + refName);
    }

    public void SendBookMark(int id, string refName)
    {
        if (sendBookMarkEvent) connectionManager.Send(ACTION_BOOK_MARK + "-" + id + "-" + refName);
    }

}

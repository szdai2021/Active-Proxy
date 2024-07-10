using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ConnectionManager : MonoBehaviour
{
    public string ipAddress = "ws://localhost:8080";

    private WebSocket ws;
    public void Connect()
    {
        ws = new WebSocket(ipAddress);
        ws.OnClose += Ws_OnClose;
        ws.OnError += Ws_OnError;
        ws.OnMessage += Ws_OnMessage;
        ws.OnOpen += Ws_OnOpen;
        ws.Connect();
    }

    public void Send(string message)
    {
        if (ws != null && ws.IsAlive)
        {
            Debug.Log(message);
            ws.Send(message);
        }
        else
        {
            Debug.LogError("Not connected!");
        }
    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
        Debug.Log(e.ToString());
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.ToString());
    }

    private void Ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.ToString());
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log(e.ToString());
    }
}

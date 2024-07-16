using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ConnectionManager : MonoBehaviour
{
    public string ipAddress = "ws://localhost:8080";

    private WebSocket ws;

    public Dictionary<int, Vector2> posReceived = new Dictionary<int, Vector2>();
    public Dictionary<int, float> rotReceived = new Dictionary<int, float>();

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
        print(e.ToString());

        // break down
        // i - index; px, py - position; r - rotation; e - event

        string strings = e.ToString();
        string[] stringLines = strings.Split(' ');

        Vector2 p = Vector2.zero;
        float r = 0;
        int i = -1;

        foreach (string s in stringLines)
        {
            if (s.StartsWith("i"))
            {
                i = int.Parse(s.Substring(1));
            }

            if (s.StartsWith("px"))
            {
                p.x = float.Parse(s.Substring(1));
            }

            if (s.StartsWith("py"))
            {
                p.y = float.Parse(s.Substring(1));
            }

            if (s.StartsWith("r"))
            {
                r = float.Parse(s.Substring(1));
            }

            if (s.StartsWith("e"))
            {
                // event trigger: to do
            }
        }

        posReceived[i] = p;
        rotReceived[i] = r;
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

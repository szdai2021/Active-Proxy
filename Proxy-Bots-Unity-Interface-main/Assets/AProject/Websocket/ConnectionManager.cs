using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Text.RegularExpressions;

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
            //Debug.Log(message);
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
        string pattern = @"marker-(\d+):\((\d+\.\d+),(\d+\.\d+),(\d+\.\d+)\)";
        MatchCollection matches = Regex.Matches(e.Data, pattern);

        foreach (Match match in matches)
        {
            if (match.Groups.Count == 5)
            {
                int index = int.Parse(match.Groups[1].Value);
                Vector2 p = new Vector2(
                    float.Parse(match.Groups[2].Value),
                    float.Parse(match.Groups[3].Value)
                );
                float angle = float.Parse(match.Groups[4].Value);

                posReceived[index] = p;
                rotReceived[index] = angle;
            }
        }
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

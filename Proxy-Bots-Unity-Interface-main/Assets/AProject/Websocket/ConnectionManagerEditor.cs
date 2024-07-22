using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEditor;

[CustomEditor(typeof(ConnectionManager))]
public class ConnectionManagerEditor : Editor
{
    public string ipAddress = "ws://localhost:8080";

    private WebSocket ws;

    public Dictionary<int, Vector2> posReceived = new Dictionary<int, Vector2>();
    public Dictionary<int, float> rotReceived = new Dictionary<int, float>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ConnectionManager myScript = (ConnectionManager)target;

        if (GUILayout.Button("re-connect"))
        {
            myScript.Connect();
        }
    }
}

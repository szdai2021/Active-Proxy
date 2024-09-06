using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMarkDataTransmitter : MonoBehaviour
{
    public APIManager apiManager;
    public BookMarkActionDetection zone;

    private void Awake()
    {
        zone.onActionEnter.AddListener(SendEnter);
    }

    private void SendEnter(int id, string refName)
    {
        apiManager.SendBookMark(id, refName);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMarkCollider : MonoBehaviour
{
    public string checkName = "ZONE_DASHBOARD";
    public bool actionDetected = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == checkName)
        {
            actionDetected = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == checkName)
        {
            actionDetected = false;
        }
    }
}

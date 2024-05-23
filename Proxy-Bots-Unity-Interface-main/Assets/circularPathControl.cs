using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularPathControl : MonoBehaviour
{
    public bool enable = false;

    public float rotationSpeed = 0.1f;

    public float rotationValue = 0;

    private void Update()
    {
        if (enable)
        {
            rotationValue = rotationSpeed;
        }
        else
        {
            rotationValue = 0;
        }

        this.transform.Rotate(0, rotationValue, 0, Space.Self);
    }
}

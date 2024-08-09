using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posClone : MonoBehaviour
{
    public GameObject parent;

    public GameObject offsetReference;

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = parent.transform.localPosition;
        this.transform.localRotation = parent.transform.localRotation;
    }
}

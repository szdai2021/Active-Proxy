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
        this.transform.position = parent.transform.position - (offsetReference.transform.position - this.transform.parent.transform.position);
        this.transform.rotation = parent.transform.rotation;
    }
}

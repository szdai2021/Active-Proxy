using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posSyc : MonoBehaviour
{
    public GameObject sycChild;

    // Update is called once per frame
    void Update()
    {
        sycChild.transform.position = this.transform.position;
        sycChild.transform.rotation = this.transform.rotation;
    }
}

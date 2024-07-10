using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;

    public float f = 0f;

    // Update is called once per frame
    void Update()
    {
        Quaternion q = Quaternion.AngleAxis(f, (b.transform.position - c.transform.position));

        a.transform.rotation *= q;

        a.transform.LookAt(b.transform, Vector3.up);

    }
}

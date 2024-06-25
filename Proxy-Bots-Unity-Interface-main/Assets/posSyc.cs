using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posSyc : MonoBehaviour
{
    public GameObject sycChild;
    public calculateTransMatrix TMcalculatation;

    public bool posSycFlag = true;
    public bool rotSycFlag = true;

    public bool VICON = false;

    private Matrix4x4 transMatrix = Matrix4x4.zero;

    //public GameObject trackedAnchor;
    //public GameObject Anchor;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        Vector3 rot = this.transform.rotation.eulerAngles;

        //Vector3 posDifference = - trackedAnchor.transform.position + Anchor.transform.position;
        //Vector3 rotDifference = -trackedAnchor.transform.rotation.eulerAngles + Anchor.transform.rotation.eulerAngles;


        if (VICON)
        {
            transMatrix = TMcalculatation.transMatrix;

            pos = transMatrix.inverse.MultiplyPoint3x4(pos);
            //rot = rot + rotDifference;
        }

        if (posSycFlag) {
            sycChild.transform.position = pos;
        }

        if (rotSycFlag)
        {
            sycChild.transform.rotation = Quaternion.Euler(rot);
        }
        
    }
}

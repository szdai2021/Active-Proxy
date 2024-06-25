using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPosMatch : MonoBehaviour
{
    public GameObject temp;
    public GameObject scynTarget;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        temp.transform.localPosition = this.transform.parent.localPosition;
        temp.transform.localRotation = this.transform.parent.localRotation;

        temp.transform.GetChild(0).localPosition = this.transform.localPosition;
        temp.transform.GetChild(0).localRotation = this.transform.localRotation;

        scynTarget.transform.position = temp.transform.GetChild(0).position;
        scynTarget.transform.rotation = temp.transform.GetChild(0).rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deselection : MonoBehaviour
{
    public GameObject agentParents;
    public GameObject deselectionIndicator;

    private int counter;
    // Update is called once per frame
    void Update()
    {
        counter = 0;

        foreach (Transform t in agentParents.transform)
        {
            if (this.GetComponent<BoxCollider>().bounds.Contains(t.position))
            {
                counter += 1;
            }
        }

        if (counter != 0)
        {
            deselectionIndicator.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            deselectionIndicator.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}

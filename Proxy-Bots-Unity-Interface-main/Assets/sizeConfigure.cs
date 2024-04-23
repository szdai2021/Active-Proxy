using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class sizeConfigure : MonoBehaviour
{
    public GameObject surface;
    public GameObject[] boundaries;

    [Header("--- Scale Unit ---", order = 1)]
    [Space(-10, order = 2)]
    [Header("The measure unit for the following variables is meter", order = 3)]
    public float x_Max;
    public float y_Max;

    // Update is called once per frame
    void Update()
    {
        surface.transform.localScale = new Vector3(x_Max, 0.05f, y_Max);


        boundaries[0].transform.localScale = new Vector3(boundaries[0].transform.localScale.x, boundaries[0].transform.localScale.y, x_Max);
        boundaries[1].transform.localScale = new Vector3(boundaries[1].transform.localScale.x, boundaries[1].transform.localScale.y, x_Max);
        boundaries[2].transform.localScale = new Vector3(boundaries[2].transform.localScale.x, boundaries[2].transform.localScale.y, y_Max);
        boundaries[3].transform.localScale = new Vector3(boundaries[3].transform.localScale.x, boundaries[3].transform.localScale.y, y_Max);

        boundaries[0].transform.localPosition = new Vector3(0, 0, -y_Max / 2);
        boundaries[1].transform.localPosition = new Vector3(0, 0, y_Max / 2);
        boundaries[2].transform.localPosition = new Vector3(-x_Max / 2, 0, 0);
        boundaries[3].transform.localPosition = new Vector3(x_Max / 2, 0, 0);
    }
}

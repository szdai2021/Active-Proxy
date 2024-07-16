using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoordinateByPivot : MonoBehaviour
{
    public GameObject pivotReference;

    public Vector2 getRelativeCoord(GameObject g)
    {
        Vector2 coord2D = Vector2.zero;

        Vector3 difference = g.transform.position - pivotReference.transform.position;

        coord2D.x = difference.x;
        coord2D.y = difference.z;

        return coord2D;
    }
}

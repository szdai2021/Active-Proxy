using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SMPickUp : SingleManipulation
{
    public Transform zonePlane;
    public GameObject cursorPrefab;
    public GameObject cursorParent;
    private float pickUpThreshold = 0.05f;
    private static int bufferLength = 90;
    public bool onSurface = false;
    public bool onAirDwell = false;
    private Queue<Vector3> lastPositions = new Queue<Vector3>(bufferLength);

    public UnityEvent OnPickUp = new UnityEvent();
    public UnityEvent OnPlaced = new UnityEvent();
    public UnityEvent OnAirDwell = new UnityEvent();

    private GameObject cursorPlaceHolder = null;

    private void Awake()
    {
        OnPickUp.AddListener(delegate
        {
            target.gameObject.GetComponent<Renderer>().material.color = Color.red;
            // create cursor
            cursorPlaceHolder = Instantiate(cursorPrefab, cursorParent.transform);
            cursorPlaceHolder.GetComponent<sendCursorPos>().name = this.transform.parent.name;
            cursorPlaceHolder.GetComponent<sendCursorPos>().StartFlag = true;
            cursorPlaceHolder.GetComponent<sendCursorPos>().posParent = this.transform.parent.gameObject;
            Debug.Log("Picked Up");
        });

        OnPlaced.AddListener(delegate
        {
            target.gameObject.GetComponent<Renderer>().material.color = Color.blue;
            // remove cursor
            if (cursorPlaceHolder != null) {
                cursorPlaceHolder.GetComponent<sendCursorPos>().StartFlag = false;
                cursorPlaceHolder.Destroy();
            }
            Debug.Log("Placed");
        });

        OnAirDwell.AddListener(delegate
        {
            target.gameObject.GetComponent<Renderer>().material.color = Color.green;
            Debug.Log("Stationary");
        });
    }

    private void Update()
    {
        if (cursorPlaceHolder != null)
        {
            cursorPlaceHolder.GetComponent<sendCursorPos>().distance = Vector3.Distance(this.transform.parent.position, cursorPlaceHolder.transform.position);
        }

        if (lastPositions.Count >= bufferLength)
        {
            lastPositions.Dequeue();
        }

        lastPositions.Enqueue(target.position);

        Evaluate();
    }
    public override void Evaluate()
    {

        Vector3 surfacePoint = zonePlane.position + Vector3.ProjectOnPlane(target.position - zonePlane.position, zonePlane.up);

        //Debug.Log(Vector3.Distance(target.position, surfacePoint));

        if (Vector3.Distance(target.position, surfacePoint) > pickUpThreshold && onSurface)
        {
            OnPickUp.Invoke();
            onSurface = false;
        }
        else if (Vector3.Distance(target.position, surfacePoint) < pickUpThreshold && !onSurface)
        {
            OnPlaced.Invoke();
            onSurface = true;
            onAirDwell = false;
        }

        //calculate movement after air
        float sumD = 0f;
        Vector3[] list = lastPositions.ToArray();
        for(int i = 0; i < list.Length-1; i++)
        {
            sumD += Vector3.Distance(list[i], list[i + 1]);
        }
        float avg = sumD / list.Length;
      //  if(target.name == "REF_LNT") Debug.Log(avg.ToString("F3") + "," + onSurface + ", " + onAirDwell);

        if (avg < 0.005f && !onSurface && !onAirDwell)
        {
            OnAirDwell.Invoke();
            onAirDwell = true;
        }

    }

    void OnDrawGizmos()
    {
        Vector3 surfacePoint = zonePlane.position + Vector3.ProjectOnPlane(target.position - zonePlane.position, zonePlane.up);

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(surfacePoint, 0.1f);
    }
}

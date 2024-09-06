using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class sendCursorPos : MonoBehaviour
{
    public ConnectionManager cm;

    public GameObject posParent;

    public GameObject referenceTopLeft;
    public GameObject referenceTopRight;
    public GameObject referenceBtmLeft;
    public GameObject cursorPlaceholder;

    public string name;
    public float distance;

    // public GameObject screenPositionHolder;

    private Thread websocketThread;

    public bool StartFlag = true;

    private float widthMax, heightMax;
    private Vector2 tempPos;

    private void Start()
    {
        cm = GameObject.FindObjectOfType<ConnectionManager>();

        referenceTopLeft = this.transform.parent.Find("TopLeftReference").gameObject;
        referenceTopRight = this.transform.parent.Find("TopRightReference").gameObject;
        referenceBtmLeft = this.transform.parent.Find("BtmLeftReference").gameObject;
        cursorPlaceholder = this.transform.parent.Find("Cursor PlaceHolder").gameObject;

        widthMax = Mathf.Abs(Vector3.Distance(referenceTopLeft.transform.localPosition, referenceTopRight.transform.localPosition));
        heightMax = Mathf.Abs(Vector3.Distance(referenceTopLeft.transform.localPosition, referenceBtmLeft.transform.localPosition));

        websocketThread = new Thread(WebSocketCommunication);
        websocketThread.Start();
    }

    private void FixedUpdate()
    {
        cursorPlaceholder.transform.position = new Vector3(cursorPlaceholder.transform.position.x, posParent.transform.position.y, posParent.transform.position.z);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, cursorPlaceholder.transform.localPosition.y, cursorPlaceholder.transform.localPosition.z);

        Transform t = this.transform;

        float x = - t.localPosition.y + referenceTopLeft.transform.localPosition.y;
        float y = - t.localPosition.z + referenceTopLeft.transform.localPosition.z;

        tempPos = new Vector2(x,y);
    }

    private void WebSocketCommunication()
    {
        while (true)
        {
            if (StartFlag)
            {
                float a = tempPos[0] / widthMax;
                float b = tempPos[1] / heightMax;

                cm.Send(
                "C" + "-" + name + "-" + ratioLimit(a)*100 + "-" + ratioLimit(b)*100 + "-" + distance);
            }
            Thread.Sleep(100);
        }
    }

    private float ratioLimit(float f)
    {
        if (f > 1)
        {
            return 1f;
        }

        if (f < 0)
        {
            return 0f;
        }

        return f;
    }
}

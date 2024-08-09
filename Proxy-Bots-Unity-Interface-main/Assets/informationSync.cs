using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class informationSync : MonoBehaviour
{
    public ConnectionManager cm;

    public GameObject robotAgent;
    public GameObject destinationParent;
    public GameObject referenceTopLeft;
    public GameObject referenceTopRight;
    public GameObject referenceBtmLeft;
    public GameObject screenPositionHolder;

    private Thread websocketThread;

    public List<bool> indexEnable;

    public bool StartFlag = false;

    private float widthMax, heightMax;
    private Dictionary<int, List<float>> agentsInfo = new Dictionary<int, List<float>>();
    public Vector3 offset;

    private IEnumerator sendMessageCoroutine()
    {
        while (true)
        {
            if (StartFlag)
            {
                // send robot position to sever
                for (int i = 0; i < 6; i++)
                {
                    Transform t = robotAgent.transform.GetChild(i);

                    float orientation = Vector3.SignedAngle(t.GetChild(1).position - t.GetChild(0).position, Vector3.forward, Vector3.up);
                    float x = -t.position.x + referenceTopLeft.transform.position.x;
                    float y = -t.position.z + referenceTopLeft.transform.position.z;

                    cm.Send(
                        "R" + i.ToString() + " " +
                        "(" + y / widthMax + "," + x / heightMax + ")" + " " +
                        orientation.ToString("F4")); // rotation later

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void Start()
    {
        //StartCoroutine(sendMessageCoroutine());

        widthMax = Mathf.Abs(Vector3.Distance(referenceTopLeft.transform.position, referenceTopRight.transform.position));
        heightMax = Mathf.Abs(Vector3.Distance(referenceTopLeft.transform.position, referenceBtmLeft.transform.position));

        websocketThread = new Thread(WebSocketCommunication);
        websocketThread.Start();
    }

    private void Update()
    {
        // send robot position to sever
        for (int i = 0; i < 6; i++)
        {
            Transform t = robotAgent.transform.GetChild(i);

            float orientation = Vector3.SignedAngle(t.GetChild(1).position - t.GetChild(0).position, Vector3.forward, Vector3.down);
            float x = -t.position.x + referenceTopLeft.transform.position.x;
            float y = -t.position.z + referenceTopLeft.transform.position.z;

            agentsInfo[i] = new List<float> { x, y, orientation};

            if (cm.posReceived.ContainsKey(i))
            {
                updateDestination(i);
            }
        }
    }

    private void WebSocketCommunication()
    {
        while (true)
        {
            if (StartFlag)
            {
                // send robot position to sever
                for (int i = 0; i < 6; i++)
                {
                    if (indexEnable[i])
                    {
                        cm.Send(
                        "R" + i.ToString() + " " +
                        "(" + agentsInfo[i][1] / widthMax + "," + agentsInfo[i][0] / heightMax + ")" + " " +
                        agentsInfo[i][2].ToString("F4"));
                    }

                    Thread.Sleep(1);
                }
                Thread.Sleep(10);
            }
            Thread.Sleep(5);
        }
    }

    private void OnApplicationQuit()
    {
        if (websocketThread != null)
        {
            websocketThread.Abort();
        }
    }

    private void updateDestination(int index)
    {
        Transform t = destinationParent.transform.GetChild(index);
        screenPositionHolder.transform.localPosition = new Vector3(referenceTopLeft.transform.localPosition.x - cm.posReceived[index].y * heightMax, screenPositionHolder.transform.localPosition.y, referenceTopLeft.transform.localPosition.z - cm.posReceived[index].x * widthMax);
        screenPositionHolder.transform.localPosition += offset;
        t.transform.position = screenPositionHolder.transform.position;

        Quaternion rotationQuaternion = Quaternion.AngleAxis(cm.rotReceived[index], Vector3.up);

        t.rotation = rotationQuaternion;            
    }
}

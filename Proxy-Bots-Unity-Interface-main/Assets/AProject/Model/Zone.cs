using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

/// <summary>
/// Zone represents chart in the dashboard
/// </summary>
public class Zone : MonoBehaviour
{
    public int zoneId;
    public List<Transform> trackedObjects = new List<Transform>();

    // less than min cut off is confirmation zone, more than max is out of scope
    public float maxCutOff;
    public float minCutOff;

    public float dwellTimeMilSec;

    public OnEnterConfirmationZone onEnter = new OnEnterConfirmationZone();
    public OnEnterTrackingZone onTracking = new OnEnterTrackingZone();
    public OnExitConfirmationZone onExit = new OnExitConfirmationZone();
    public OnDwelledConfirmationZone onDwelled = new OnDwelledConfirmationZone();
    public OnDwellingConfirmationZone onDwelling = new OnDwellingConfirmationZone();


    private Material mat;
    private Dictionary<Transform, Stopwatch> dwellList = new Dictionary<Transform, Stopwatch>();
    private Dictionary<Transform, bool> confirmationZoneList = new Dictionary<Transform, bool>();
    private Dictionary<Transform, bool> dwelledList = new Dictionary<Transform, bool>();

    private Dictionary<string, bool> prev_entered = new Dictionary<string, bool>();

    private void Start()
    {
        foreach (Transform t in trackedObjects)
        {
            prev_entered[t.name] = false;
        }

        mat = gameObject.GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
       // CheckDistance();
        CheckDwellList();

        foreach (Transform t in trackedObjects)
        {
            if (!prev_entered[t.name])
            {
                if (this.GetComponent<BoxCollider>().bounds.Contains(t.position))
                {
                    // building has entered the zone

                    if (t.CompareTag("proxy"))
                    {
                        if (!confirmationZoneList.ContainsKey(t))
                        {
                            confirmationZoneList.Add(t, false);
                        }
                        confirmationZoneList[t] = true;
                        Debug.Log("Enter confirmation zone " + t.name);
                        mat.color = Color.green;
                        onEnter.Invoke(zoneId, t.name);
                        AddToDwellList(t);
                        dwellList[t].Restart();
                    }

                    prev_entered[t.name] = true;
                }
            }

            if (prev_entered[t.name])
            {
                if (!this.transform.GetChild(0).GetComponent<BoxCollider>().bounds.Contains(t.position))
                {
                    // building has exited the zone

                    if (t.CompareTag("proxy"))
                    {
                        if (!confirmationZoneList.ContainsKey(t))
                        {
                            confirmationZoneList.Add(t, false);
                        }
                        Debug.Log("Exit confirmation zone " + t.name);
                        confirmationZoneList[t] = false;
                        onExit.Invoke(zoneId, t.name);
                        dwellList[t].Stop();
                        dwelledList[t] = false;
                    }

                    prev_entered[t.name] = false;
                }
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("proxy"))
        {
            if (!confirmationZoneList.ContainsKey(t))
            {
                confirmationZoneList.Add(t, false);
            }
            confirmationZoneList[t] = true;
            Debug.Log("Enter confirmation zone " + t.name);
            mat.color = Color.green;
            onEnter.Invoke(zoneId, t.name);
            AddToDwellList(t);
            dwellList[t].Restart();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("proxy"))
        {
            if (!confirmationZoneList.ContainsKey(t))
            {
                confirmationZoneList.Add(t, false);
            }
            Debug.Log("Exit confirmation zone " + t.name);
            confirmationZoneList[t] = false;
            onExit.Invoke(zoneId, t.name);
            dwellList[t].Stop();
            dwelledList[t] = false;
        }
    }
    */

    [Obsolete("CheckDistance is deprecated.")]
    private void CheckDistance()
    {
        foreach (Transform t in trackedObjects)
        {
            float d = Vector3.Distance(transform.position, t.position);
            if (!confirmationZoneList.ContainsKey(t))
            {
                confirmationZoneList.Add(t, false);
                
            }

            if (d < maxCutOff)
            {
                // entering zone
                if (d <= minCutOff && !confirmationZoneList[t])
                {
                    confirmationZoneList[t] = true;
                    Debug.Log("Enter confirmation zone " + t.name);
                    mat.color = Color.green;
                    onEnter.Invoke(zoneId, t.name);
                    AddToDwellList(t);
                    dwellList[t].Restart();
                }
                // exiting zone
                else if (d > minCutOff && confirmationZoneList[t])
                {
                    Debug.Log("Exit confirmation zone " + t.name);
                    confirmationZoneList[t] = false;
                    onExit.Invoke(zoneId, t.name);
                    dwellList[t].Stop();
                    dwelledList[t] = false;
                }
                else
                {
                    //Debug.Log(t.name + " Distance =" + d.ToString("F2"));
                    mat.color = Color.Lerp(Color.red, Color.green, minCutOff / d);
                    onTracking.Invoke(zoneId, minCutOff / d, t.name);
                }
            }
            else
            {
                mat.color = Color.black;
            }
        }
    }
    private void CheckDwellList()
    {
        foreach (KeyValuePair<Transform, Stopwatch> t in dwellList)
        {
            if(confirmationZoneList[t.Key])
            {
                float milSec = t.Value.ElapsedMilliseconds;
                //Debug.Log(milSec);

                if (milSec >= dwellTimeMilSec)
                {
                    if (!dwelledList[t.Key])
                    {
                        onDwelled.Invoke(zoneId, t.Key.name);
                        dwelledList[t.Key] = true;
                    }
                }
                else 
                {                    
                    float norm = milSec / dwellTimeMilSec;
                    onDwelling.Invoke(zoneId, t.Key.name, norm);
                    //Debug.Log(zoneId + "-" + norm);
                }
            }
           
        }
    }

    private void AddToDwellList(Transform t)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (!dwellList.ContainsKey(t))
        {
            dwellList.Add(t, stopwatch);
            dwelledList.Add(t, false);
        }
    }

    private void ATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }
}


[System.Serializable]
public class OnEnterConfirmationZone : UnityEvent<int, string>
{
}

public class OnExitConfirmationZone: UnityEvent<int, string>
{

}

public class OnDwellingConfirmationZone : UnityEvent<int, string, float>
{

}

public class OnDwelledConfirmationZone : UnityEvent<int, string>
{

}

[System.Serializable]
public class OnEnterTrackingZone : UnityEvent<int, float, string>
{
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A simple script to control the virtual robot rotation and movement using iterative approach
/// </summary>
public class VirtualRobotNavigation : MonoBehaviour
{

    public int iterationRate = 100;

    public float initialRotationStep = 0.1f;
    public float initialMovementStep = 0.01f;

    public bool alignDirection = false;

    private float currentRotationStep;
    private float currentMovementStep;
    private float currentRotationThreshold;

    private Vector3 targetDirection;

    public UnityEventVirtualRobotNavMove OnMove;
    public UntiyEventVirtualRobotNavRotate OnRotate;
    public UnityEvent OnStop;

    private void Rotate(float angle)
    {
        transform.Rotate(transform.up, angle);
        OnRotate.Invoke(angle);
    }

    private void Move(float amount)
    {
        transform.Translate(Vector3.forward * amount, Space.Self);
        OnMove.Invoke(amount);
    }

    private bool IsAlign(Vector3 point)
    {
        Vector3 dir = (point - transform.position).normalized;
        Vector2 dir2D = new Vector3(dir.x, dir.z);
        Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z);

        float angle = Vector2.Angle(forward2D, dir2D);
        //Debug.Log("Current Angle = " + angle);
        //Debug.Log("Threshold = " + currentRotationThreshold);
        return (angle < currentRotationThreshold);
    }

    /// <summary>
    /// Determine when the Navigation itteration will stop
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool IsStopPoint(Vector3 point)
    {
        Vector2 point2D = new Vector2(point.x, point.z);
        Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

        return (Vector2.Distance(point2D, pos2D) < 0.1f);
    }

    private bool IsApproaching(Vector3 point)
    {
        Vector2 point2D = new Vector2(point.x, point.z);
        Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

        float dot = Vector2.Dot(transform.forward, (point2D - pos2D).normalized);
        return ((dot >= 0) && !IsStopPoint(point));
    }

    private float CalculateAngle(Vector3 point)
    {
        Vector2 point2D = new Vector2(point.x, point.z);
        Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z);
        return Vector2.Angle(forward2D, (point2D - pos2D).normalized);
    }

    public void Navigate(Vector3 point, Vector3 direction)
    {
        StopAllCoroutines();
        targetDirection = direction;
        currentRotationStep = initialRotationStep;
        currentMovementStep = initialMovementStep;
        currentRotationThreshold = CalculateAngle(point) * 0.05f;
        NavigateIterate(point);
    }

    //public float CalculateMinDistance(Vector3 point, float angle)
    //{
    //    Vector2 point2D = new Vector2(point.x, point.z);
    //    Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
    //    return Vector2.Distance(pos2D, point2D) * Mathf.Sin(Mathf.Deg2Rad * angle);        
    //}

    private void NavigateIterate(Vector3 point)
    {
        if (!IsStopPoint(point))
        {
            Debug.Log("<color='blue'>Rotating</color>");
            RotateToward(point, delegate
            {
                Debug.Log("<color='blue'>Moving</color>");
                MoveToward(point, delegate
                {
                    currentMovementStep = currentMovementStep - (currentMovementStep * 0.1f);
                    currentRotationStep = currentRotationStep - (currentRotationStep * 0.1f);
                    currentRotationThreshold *= 0.5f;
                    NavigateIterate(point);
                });
            });
        }
        else
        {
            if (alignDirection)
            {
                RotateToward(transform.position + targetDirection, delegate
                {
                });
            }
            else
            {
                OnStop.Invoke();
            }

        }

    }

    public void MoveToward(Vector3 point, Action callback)
    {
        StartCoroutine(MoveTowardIteration(point, callback));
    }

    public void RotateToward(Vector3 point, Action callback)
    {
        StartCoroutine(RotateTowardIteration(point, callback));
    }

    //private float CalculateDistance(Vector3 point)
    //{
    //    Vector2 point2D = new Vector2(point.x, point.z);
    //    Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
    //    float distance = Vector2.Distance(point2D, pos2D);
    //    return distance;
    //}

    IEnumerator MoveTowardIteration(Vector3 point, Action callback)
    {
        if (IsAlign(point))
        {            
            while (IsApproaching(point))
            {
                Move(currentMovementStep);
                yield return new WaitForSeconds(1f/iterationRate);
            }
            callback();
        }
    }

    IEnumerator RotateTowardIteration(Vector3 point, Action callback)
    {
        while (!IsAlign(point))
        {
            Rotate(currentRotationStep);
            yield return new WaitForSeconds(1f/iterationRate);
        }
        callback();
    }
}

[Serializable]
public class UnityEventVirtualRobotNavMove : UnityEvent<float> { }
[Serializable]
public class UntiyEventVirtualRobotNavRotate: UnityEvent<float> { }
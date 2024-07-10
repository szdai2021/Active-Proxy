using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SMRotate : SingleManipulation
{

    public UnityEvent OnPitchExit = new UnityEvent();
    public UnityEvent OnPitchForward = new UnityEvent();
    public UnityEvent OnPitchBackward = new UnityEvent();

    public UnityEvent OnRollExit = new UnityEvent();
    public UnityEvent OnRollRight = new UnityEvent();
    public UnityEvent OnRollLeft = new UnityEvent();

    public float minAngle = 50f;

    private bool isPitching;
    private bool isRolling;
    private void Awake()
    {
        OnPitchForward.AddListener(delegate
        {
            Debug.Log("<color='blue'>Pitch Enter Forward</color>");
        });

        OnPitchBackward.AddListener(delegate
        {
            Debug.Log("<color='blue'>Pitch Enter Backward</color>");
        });

        OnPitchExit.AddListener(delegate
        {
            Debug.Log("<color='red'>Pitch Exit</color>");
        });

        OnRollRight.AddListener(delegate
        {
            Debug.Log("<color='yellow'>Roll Enter Right</color>");
        });

        OnRollLeft.AddListener(delegate
        {
            Debug.Log("<color='yellow'>Roll Enter Left</color>");
        });

        OnRollExit.AddListener(delegate
        {
            Debug.Log("<color='red'>Roll Exit</color>");
        });
    }

    public override void Evaluate()
    {
        float pitchAngle = 90f - Vector3.SignedAngle(Vector3.up, target.forward, Vector3.right);
        float rollAngle = Vector3.SignedAngle(Vector3.right, target.right, Vector3.forward);

        CheckPitch(pitchAngle);
        CheckRoll(rollAngle);

    }

    private void CheckRoll(float rollAngle)
    {
        if (Mathf.Abs(rollAngle) > minAngle)
        {
            if (!isRolling)
            {
                isRolling = true;
                if (rollAngle < 0)
                {
                    OnRollRight.Invoke();
                }
                else
                {
                    OnRollLeft.Invoke();
                }
            }

        }
        else if (isRolling)
        {
            isRolling = false;
            OnRollExit.Invoke();
        }
    }

    private void CheckPitch(float pitchAngle)
    {

        if (Mathf.Abs(pitchAngle) > minAngle)
        {
            if (!isPitching)
            {
                isPitching = true;
                if (pitchAngle < 0)
                {
                    OnPitchForward.Invoke();
                }
                else
                {
                    OnPitchBackward.Invoke();
                }
            }

        }
        else if (isPitching)
        {
            isPitching = false;
            OnPitchExit.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Evaluate();
    }
}

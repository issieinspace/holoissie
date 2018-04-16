using UnityEngine;
using Timers;
using System;
using UnityEngine.Events;

public class CEVISExerciseMove : ExerciseMove {

    WebAPIBridge Bridge;
    public float startDistance = 0.0f;
    public float currentDistance;
    public float standardDistanceUnit = 10.0f;
    public float interval = 0.5f;
    public uint loops = 100;

    protected override void setup()
    {
        base.setup();
        // Wait for the appropriate time before starting
        Bridge = GameObject.Find("CycleDataReader").GetComponent<WebAPIBridge>();
        TimersManager.SetTimer(this, interval, loops, RefreshDistance);
    }

    void RefreshDistance()
    {
        Debug.Log("Refreshing distance");
        Bridge.GetCycleReadout();
    }

    protected override bool checkForDisplacement()
    {
        bool complete = false;

        if (Input.GetAxis("Jump") > 0.0f)
        {
            complete = true;
        }
        else
        {
            complete = checkDistanceAchieved(standardDistanceUnit);
        }

        return complete;
    }

    protected override bool checkForCompletion()
    {
        bool complete = false;
       
        if (Input.GetAxis("Jump") > 0.0f)
        {
            complete = true;
        }
        else
        {
            complete = checkDistanceAchieved(standardDistanceUnit * 2);
        }

        return complete;
    }

    private bool checkDistanceAchieved(float testDistance)
    {
        currentDistance = Bridge.cycleReadout.distance;

        Debug.Log("Checking distance... start: " + startDistance + " current: " + currentDistance + " test:" + testDistance);

        if (startDistance == 0.0f)
        {
            startDistance = currentDistance;
        }
        else
        {
            return currentDistance - startDistance > testDistance;
        }

        return false;
    }
}

using UnityEngine;
using System.Collections;
using Timers;
using System;

public class ExerciseTimer : MonoBehaviour {

    void Start()
    {
        GetComponent<TextMesh>().text = "";

    }

    // Update is called once per frame
    void Update () {
        float timeRemaining = TimersManager.RemainingTime(OnDone);
        if (timeRemaining > 0)
        {
            //Debug.Log("exercise timer " + timeRemaining);
            // (ExerciseTimer, TimeForExercise, 1, ExerciseTimer.OnDone);
            GetComponent<TextMesh>().text = timeRemaining.ToString("N1");
        }
       
        
    }

    public void TriggerStart(float TimeForExercise)
    {

        TimersManager.SetTimer(this, TimeForExercise, 1, OnDone);

    }

    public void OnDone()
    {
        GetComponent<TextMesh>().text = "";
    }

    
}

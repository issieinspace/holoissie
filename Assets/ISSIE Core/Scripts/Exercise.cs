﻿using UnityEngine;
using System.Collections;
using System;
using Timers;
using Prime31.MessageKit;

public class Exercise : MonoBehaviour, ITriggerable
{
    // --- Configuration
    private string ExerciseName;
    public int Order;

    // The time
    public float TimeForExercise = 30;
    public int MovesForAchievement;
   
    // The move for this exercise
    public GameObject ExerciseMovePrefab;
    public System.Collections.Generic.List<ExerciseMove> Moves;
    public bool ReckonStartPositionFromReadyPositions = true;
    public Vector3 OriginalPosition;
    
    public bool PlayerIsReady = true;
    public int AchievementCount = 0;
    public bool ExerciseComplete = false;
    public bool ExerciseInProgress = false;
    public bool TriggeredFirstAchievementAttained = false;
    public bool TriggeredAlmostDone = false;
    public bool CountDownDone = false;
    public bool CountDownStarted = false;


    public float TimeLeft;
    public int MovesCompleted = 0;
    ExerciseMove Move;

    public uint TicksToCountDown = 6;
    public uint CountDown;
    public ExerciseTimer ExerciseTimer;
    public bool isSetup = false;

    void Start()
    {
        Debug.Log("In Start() of Exercise, " + this.name);

        if (!isSetup)
        {
            Setup();
        }
    }

    public void Setup()
    {
        ExerciseName = this.name;

        TimeLeft = TimeForExercise;

        CountDown = TicksToCountDown;

        this.GetComponent<PropActivator>().Setup();

        isSetup = true;
    }

    void Update()
    {
        if (ExerciseInProgress)
        {
            // Test for move completed
            if (Move.Complete)
            {
                HandleMoveComplete();
            }
            else if (Move.DisplacementAchieved && !Move.TriggeredDisplacementAchieved)
            {
                HandleDisplacementAchieved();
            }
            
             // Test for time up
            TimeLeft -= Time.deltaTime;

            if (TimeLeft < 8 && !TriggeredAlmostDone)
            {
                TriggeredAlmostDone = true;
                BroadcastAlmostDone();
            }

            if (TimeLeft < 0)
            {
                StopExercise();
            }
        }
 
    }

    void HandleMoveComplete()
    {
        BroadcastMoveComplete();
        Moves.Add(Move);
        MovesCompleted++;
             
        // Test for achievement unlocked
        if (Moves.Count > 0 && (Moves.Count % MovesForAchievement == 0))
        {
            Debug.Log(ExerciseName + "GOT AN ACHIEVEMENT");
            BroadcastAchievement();
            if(AchievementCount == 0)
            {
                BroadcastFirstAchievement();
            }
            AchievementCount++;
        }

        CreateNewMove();
    }

    void HandleDisplacementAchieved()
    {
        BroadcastDisplacementAchieved();
        Move.TriggeredDisplacementAchieved = true;
    }

    void PlayerReady()
    {
        if (!CountDownStarted)
        {
            Debug.Log("In PlayerReady() of Exercise. CountDown not started, so performing ReadyExercise");
            ReadyExercise();
        }

        //Timers handle the start now, so this code is obviated
        //if (CountDownDone)
        //{
        //    if (!ExerciseComplete)
        //    {
        //        Debug.Log("In PlayerReady() of Exercise. CountDownDone is true and ExerciseComplete false, so performing ReadyExercise");
        //        StartExercise();
        //    }
        //}
    }

    // Ready exercise
    void ReadyExercise()
    {
        // Clear previous moves
        Moves = new System.Collections.Generic.List<ExerciseMove>();
        
        TimersManager.SetTimer(this, 1f, TicksToCountDown, TriggerTick);
        TimersManager.SetTimer(this, TicksToCountDown, StartExercise);
 
        CountDownStarted = true;

        BroadcastExerciseReady();

        Debug.Log(ExerciseName + ": Ready Exercise has completed");
    }

    void TriggerTick()
    {
        CountDown--;
        BroadcastTriggerTick(CountDown);
    }


    // Start the exercise
    void StartExercise()
    {
        CountDownDone = true;
        ExerciseInProgress = true;
        OriginalPosition = Camera.main.transform.position;

        CreateNewMove();
        BroadcastExerciseStart();
        

        ExerciseTimer = GameObject.Find("ExerciseTimer").GetComponent<ExerciseTimer>();
        ExerciseTimer.OnStart(TimeForExercise);
        Debug.Log(ExerciseName + ": Started Exercise");
    }
    
    void StopExercise()
    {
        ExerciseInProgress = false;
        ExerciseComplete = true;

        BroadcastTimeDone();

        Debug.Log(ExerciseName + ": Stopped Exercise");
    }

    private void CreateNewMove()
    {
        Move = Instantiate(ExerciseMovePrefab).GetComponent<ExerciseMove>();
       
        if(ReckonStartPositionFromReadyPositions)
        {
            Debug.Log("Using original position - " + Move.originalPosition.x +"," + Move.originalPosition.x + ","+ Move.originalPosition.x);
            Move.originalPosition = OriginalPosition;
        }
        else
        {
            Debug.Log("Using camera position");
            Move.originalPosition = Camera.main.transform.position;
        }
        
        Debug.Log(ExerciseName + "Move created");
    }

    public string getStatus()
    {
        if (Move == null)
        {
            return ExerciseName + "No Move setup yet";
        }
        else
        {
            return ExerciseName + " - " + Move.getMoveName() + "\n\rDisplacement "
                     + Move.getXDisplacement().ToString("N3") + ","
                     + Move.getYDisplacement().ToString("N3") + ","
                     + Move.getZDisplacement().ToString("N3") + "," + "|" + "\n\r"
                     + "Displaced? " + Move.getDisplacementAchieved() + "\n\r"
                     + "Targets: " + Move.getTargetXDisplacement() + ", " + Move.getTargetYDisplacement() + ", " + Move.getTargetZDisplacement();

        }
    }

    void BroadcastDisplacementAchieved()
    {
        MessageKit.post(MessageType.OnDisplacementAchieved);
        //SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnDisplacementAchieved"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastMoveComplete()
    {
        MessageKit.post(MessageType.OnMoveComplete);
        //SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnMoveComplete"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastAchievement()
    {
        MessageKit.post(MessageType.OnAchievement);
        //SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnAchievement"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastFirstAchievement()
    {
        MessageKit.post(MessageType.OnFirstAchievement);
        //SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnFirstAchievement"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastExerciseReady()
    {
        Debug.Log("Broadcasting ExerciseReady" + ExerciseName);
        MessageKit<string>.post(MessageType.OnReady, ExerciseName);
        Debug.Log("Broadcasted ExerciseReady" + ExerciseName);
    }

    void BroadcastAlmostDone()
    {
        MessageKit.post(MessageType.OnAlmostDone);
        //Hashtable args = packArgs(null, "methodName", "OnAlmostDone");
        //args = packArgs(args, "exerciseName", ExerciseName);
        //SendMessageUpwards("HandleEvent", args, SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastExerciseStart()
    {
        MessageKit.post(MessageType.OnStart);
        //SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnStart"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastTriggerTick(uint countDown)
    {
        MessageKit<uint>.post(MessageType.OnTick, countDown);
        //Hashtable args = packArgs(null, "methodName", "OnTick");
        //args = packArgs(args, "countDown", countDown);
        //SendMessageUpwards("HandleEvent", args, SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastTimeDone()
    {
        MessageKit.post(MessageType.OnTimeDone);
        //SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnTimeDone"), SendMessageOptions.DontRequireReceiver);
    }

    //Hashtable packArgs(Hashtable args, String key, object value)
    //{
    //    if (args == null)
    //    {
    //        args = new Hashtable();
    //    }

    //    args.Add(key, value);

    //    return args;
    //}

    public void Activate()
    {
        PlayerReady();
    }

    public void Deactivate()
    {
        //no-op;
    }
}
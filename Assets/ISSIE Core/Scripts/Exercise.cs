using UnityEngine;
using System.Collections;
using System;
using Timers;

public class Exercise : MonoBehaviour
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
    public bool ReckonStartPositionFromReadyPositions;

    // AudioClips to use for marking progress
    public string achievementSoundName = "EtherealAccent";
    public string timeDoneSoundName = "SynthZap";
    public string firstAchievementSoundName = "NICEWORK1-Remix";
    public string almostDoneSoundName = "YourAlmostThere1-Remix"; 
    // ---

    // State related info
    public AudioClip firstAchievementSound = null;
    public AudioClip almostDoneSound = null;

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

    void Start()
    {
        ExerciseName = this.name;
        firstAchievementSound = Resources.Load<AudioClip>(firstAchievementSoundName);
        almostDoneSound = Resources.Load<AudioClip>(almostDoneSoundName);

        TimeLeft = TimeForExercise;

        CountDown = TicksToCountDown;
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

            if (TimeLeft < 0)
            {
                StopExercise();
            }
        }
        else
        {
            if (PlayerIsReady)
            {
                if (!CountDownStarted)
                {
                    ReadyExercise();
                }

                if (CountDownDone)
                {
                    if (!ExerciseComplete)
                    {
                        StartExercise();
                    }
                }
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
            AchievementCount++;
        }

        CreateNewMove();
    }

    void HandleDisplacementAchieved()
    {
        BroadcastDisplacementAchieved();
        Move.TriggeredDisplacementAchieved = true;
    }

    // Ready exercise
    void ReadyExercise()
    {
        // Clear previous moves
        Moves = new System.Collections.Generic.List<ExerciseMove>();

        BroadcastExerciseReady();

        TimersManager.SetTimer(this, 1f, TicksToCountDown, TriggerTick);
        TimersManager.SetTimer(this, TicksToCountDown, StartExercise);
 
        CountDownStarted = true;
        
        Debug.Log(ExerciseName + ": Ready Exercise");
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
        CreateNewMove();
        BroadcastExerciseStart();
        //TransporterBehaviour.OnStart();

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
        Vector3 originalPosition = Camera.main.transform.position;
        Move = Instantiate(ExerciseMovePrefab).GetComponent<ExerciseMove>();
       
        if(ReckonStartPositionFromReadyPositions)
        {
            Move.originalPosition = originalPosition;
        }
        else
        {
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
            return ExerciseName + " - " + Move.getMoveName() + "\n\r"
                     + Move.getXDisplacement().ToString("N3") + ","
                     + Move.getYDisplacement().ToString("N3") + ","
                     + Move.getZDisplacement().ToString("N3") + "," + "|" + "\n\r"
                     + "Displaced? " + Move.getDisplacementAchieved() + "\n\r"
                     + "Targets: " + Move.getTargetXDisplacement() + ", " + Move.getTargetYDisplacement() + ", " + Move.getTargetZDisplacement();

        }
    }

    void BroadcastDisplacementAchieved()
    {
        SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnDisplacementAchieved"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastMoveComplete()
    {
        SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnMoveComplete"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastAchievement()
    {
        SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnAchievement"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastExerciseReady()
    {
        Hashtable args = packArgs(null, "methodName", "OnReady");
        args = packArgs(args, "exerciseName", ExerciseName);
        SendMessageUpwards("HandleEvent", args, SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastExerciseStart()
    {
        SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnStart"), SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastTriggerTick(uint countDown)
    {
        Hashtable args = packArgs(null, "methodName", "OnTick");
        args = packArgs(args, "countDown", countDown);
        SendMessageUpwards("HandleEvent", args, SendMessageOptions.DontRequireReceiver);
    }

    void BroadcastTimeDone()
    {
        SendMessageUpwards("HandleEvent", packArgs(null, "methodName", "OnTimeDone"), SendMessageOptions.DontRequireReceiver);
    }

    Hashtable packArgs(Hashtable args, String key, object value)
    {
        if (args == null)
        {
            args = new Hashtable();
        }

        args.Add(key, value);

        return args;
    }

}
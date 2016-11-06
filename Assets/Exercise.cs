using UnityEngine;
using System.Collections;
using System;
using Timers;

public class Exercise : MonoBehaviour
{

    // --- Configuration
    public GameObject TransporterControl;
    public string ExerciseName;
    public int Order;

    // The time
    public float TimeForExercise = 30;
    public int MovesForAchievement;
    TransporterBehaviour TransporterBehaviour;
   
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
    AudioClip achievementSound = null;
    AudioClip timeDoneSound = null;
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

    // Use this for initialization
    void Start()
    {
        achievementSound = Resources.Load<AudioClip>(achievementSoundName);
        timeDoneSound = Resources.Load<AudioClip>(timeDoneSoundName);
        firstAchievementSound = Resources.Load<AudioClip>(firstAchievementSoundName);
        almostDoneSound = Resources.Load<AudioClip>(almostDoneSoundName);

        TimeLeft = TimeForExercise;
        TransporterBehaviour = TransporterControl.GetComponent<TransporterBehaviour>();

        CountDown = TicksToCountDown;
       

    }

    void Update()
    {
        if (ExerciseInProgress)
        {
            // Test for move completed
            if (Move.Complete)
            {
                TransporterBehaviour.TriggerUp();
                Moves.Add(Move);
                MovesCompleted++;
                // Test for achievement unlocked
                if (Moves.Count > 0 && (Moves.Count % MovesForAchievement == 0))
                {
                    Debug.Log(ExerciseName + "GOT AN ACHIEVEMENT");
                    TransporterBehaviour.TriggerAchievement();
                    AchievementCount++;
                    // If this is the first achievement do something special
                    // TriggerFirstAchievement
                }

                CreateNewMove();
            }
            else if (Move.DisplacementAchieved && !Move.TriggeredDisplacementAchieved)
            {
                TransporterBehaviour.TriggerDown();
                Move.TriggeredDisplacementAchieved = true;
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

                if (!CountDownDone)
                {
                    // run the countdown
                    //Debug.Log("This much left: " + TimersManager.RemainingTime(StartExercise) + " elapsed: " + TimersManager.ElapsedTime(StartExercise));
                }
                else
                {
                    if (!ExerciseComplete)
                    {
                        StartExercise();
                    }
                }

            }

           
        }
    }

    public string getStatus()
    {
        if(Move == null)
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

    // Ready exercise
    void ReadyExercise()
    {
        // Clear previous moves
        Moves = new System.Collections.Generic.List<ExerciseMove>();

        // Activate transporter
        TransporterBehaviour.TriggerReady();
        TimersManager.SetTimer(this, 1f, TicksToCountDown, TriggerTick);
        TimersManager.SetTimer(this, TicksToCountDown, StartExercise);
 
        CountDownStarted = true;
        
        Debug.Log(ExerciseName + ": Ready Exercise");
    }

    void TriggerTick()
    {
        CountDown--;
        TransporterBehaviour.TriggerTick(CountDown);
    }


    // Start the exercise
    void StartExercise()
    {
        CountDownDone = true;
        ExerciseInProgress = true;
        CreateNewMove();
        TransporterBehaviour.TriggerStart();
        ExerciseTimer = GameObject.Find("ExerciseTimer").GetComponent<ExerciseTimer>();
        ExerciseTimer.TriggerStart(TimeForExercise);
        Debug.Log(ExerciseName + ": Started Exercise");
    }

  
    void StopExercise()
    {
        ExerciseInProgress = false;
        ExerciseComplete = true;

        TransporterBehaviour.TriggerTimeDone();

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

}
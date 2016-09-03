using UnityEngine;
using System.Collections;
using System;

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

    // AudioClips to use for marking progress
    public string achievementSoundName = "EtherealAccent";
    public string timeDoneSoundName = "SynthZap";
    public string encouragementSoundName = "NICEWORK1-Remix";
    public string almostDoneSoundName = "YourAlmostThere1-Remix"; 
    // ---

    // State related info
    AudioClip achievementSound = null;
    AudioClip timeDoneSound = null;
    AudioClip encouragementSound = null;
    AudioClip almostDoneSound = null;

    public bool PlayerIsReady = true;
    public int AchievementCount = 0;
    public bool ExerciseComplete = false;
    public bool ExerciseInProgress = false;
    bool TriggeredFirstAchievementAttained = false;
    public float TimeLeft;
    public int MovesCompleted = 0;
    ExerciseMove Move;
 

    // Use this for initialization
    void Start()
    {
        achievementSound = Resources.Load<AudioClip>(achievementSoundName);
        timeDoneSound = Resources.Load<AudioClip>(timeDoneSoundName);
        encouragementSound = Resources.Load<AudioClip>(encouragementSoundName); ;
        almostDoneSound = Resources.Load<AudioClip>(almostDoneSoundName);
        TimeLeft = TimeForExercise;
        TransporterBehaviour = TransporterControl.GetComponent<TransporterBehaviour>();
    }

    // Update is called once per frame
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
                    Debug.Log(ExerciseName + "I GOT AN ACHIEVEMENT");
                    TransporterBehaviour.TriggerAchievement();
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
            if (!ExerciseComplete && PlayerIsReady)
            {
                StartExercise();
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
            return ExerciseName + " " + Move.xDisplacement.ToString("N3") + ","
                     + Move.yDisplacement.ToString("N3") + ","
                     + Move.zDisplacement.ToString("N3") + "," + "|"
                     + Move.DisplacementAchieved;

        }
    }

    // Start the exercise
    void StartExercise()
    {
        // Clear previous moves
        Moves = new System.Collections.Generic.List<ExerciseMove>();
        // Start the timer
        ExerciseInProgress = true;
        CreateNewMove();
        TransporterBehaviour.TransporterActive = true;
        TransporterBehaviour.OnStart();
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
        Move = Instantiate(ExerciseMovePrefab).GetComponent<ExerciseMove>();
        Move.TransporterControl = TransporterControl;
        Debug.Log(ExerciseName + "Move created");
    }

    public void OnReset()
    {
        Debug.Log("Resetting exercise: " + ExerciseName);
        ExerciseComplete = false;
        ExerciseInProgress = false;
        PlayerIsReady = false;
        TimeLeft = TimeForExercise;
    }
}
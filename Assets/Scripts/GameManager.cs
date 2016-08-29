using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public bool RunIntro = true;
    public bool Played8SecRemaining = false;
    public bool PlayedFirstAchievement = false;
    AudioClip completionCongrats = null;
    AudioClip partyMusic = null;

    AudioSource audioSource;

    public GameObject GamePrefab; // this is the prefab from which we will spawn games
    public GameObject CurrentGame; // this is the game in progress
    public GameObject[] exercises;
    public int currentExerciseIndex = 0;

    public Exercise CurrentExercise;
    public bool ExerciseStageDone;
    public bool GameOver;
    public bool ReadyToRestart;

    public GameObject Diagnostics;


    // Use this for initialization
    void Start () {
        
        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.gameObject.SetActive(false);
        if (RunIntro)
        {
            GetComponent<AudioSource>().Play();
        }

        // Setup some audio clips
        partyMusic = Resources.Load<AudioClip>("ISSIE Game Loop - Continuous Drums");

        // We will have multiple game stages and move one to the next
        CommenceExerciseStage();
        
    }

    // Update is called once per frame
    void Update () {
        RunExerciseStage();
       
    }

   
    private void RunExerciseStage()
    {
        if (!ExerciseStageDone)
        {
            //Check if the exercise is done
            if (CurrentExercise.ExerciseComplete)
            {
                if(currentExerciseIndex + 1 >= exercises.Length)
                {
                    ExerciseStageDone = true;
                }
                else
                {
                    currentExerciseIndex++;
                    CurrentExercise = exercises[currentExerciseIndex].GetComponent<Exercise>();
                    CurrentExercise.PlayerIsReady = true;
                  
                    Debug.Log("Next exercise kicked off");
                }
            }

            OutputDiagnostics(CurrentExercise);
            // if all done, stage is done
        }
        else
        {
            if (!GameOver) //GameOver is set in HaveAParty()
            {
                HaveAParty();
            }
        }
    }

    private void OutputDiagnostics(Exercise currentExercise)
    {
        Monitor monitor = Diagnostics.GetComponent<Monitor>();
        monitor.DisplayMessage(currentExercise.getStatus());
    }

    void CommenceExerciseStage()
    {
        // Collect the exercises
        exercises = GameObject.FindGameObjectsWithTag("Exercise");
        Debug.Log("Found " + exercises.Length + " exercises.");
        CurrentExercise = exercises[currentExerciseIndex].GetComponent<Exercise>();
        CurrentExercise.PlayerIsReady = true;
        Debug.Log("ExerciseStageStarted");   
    }

    void RestartGame()
    {
        // Tell the everyone to restart via broadcast
        this.BroadcastMessage("OnReset");
    }

    void HaveAParty()
    {
        GameObject[] alienFriends = GameObject.FindGameObjectsWithTag("Alien");
        Debug.Log("You got THIS many aliens rescued: " + alienFriends.Length);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = partyMusic;
        audioSource.Play();

        foreach (GameObject alien in alienFriends)
        {
            if (!alien.GetComponent<AlienBehaviour>().isdropped)
            {
                alien.GetComponent<AlienBehaviour>().OnDrop();
            }

            alien.GetComponent<AlienBehaviour>().DanceParty();
        }
        GameOver = true;
    }


/*
    private void oldUpdateCode()
    {
        // Get the position
        Vector3 transporter = Transporter.transform.position;
        Vector3 camera = Camera.main.transform.position;
        Vector3 yDifference = transporter - camera;

        TransporterBehaviour behaviour = Transporter.GetComponent<TransporterBehaviour>();
        AudioSource audioSource = GetComponent<AudioSource>();

        //Debug.Log("Camera " + camera.y + " Transporter " + transporter.y + " Difference " + yDifference.y);

        // Test for movement
        if (behaviour.TransporterActive)
        {
            if (yDifference.y > .5)
            {
                // Get Transporter behaviour script and call methods on it
                behaviour.TriggerDown();
            }
            else
            {
                behaviour.TriggerUp();
            }

            // Test getting an achievement
            if (behaviour.AchievementCount == 1 && !PlayedFirstAchievement)
            {
                Debug.Log("About to play achievement 1");
                audioSource.clip = firstAchievement;
                audioSource.Play();
                PlayedFirstAchievement = true;
            }

            // Test for 8 seconds remaining
            if (behaviour.TimeLeft < 8 && !Played8SecRemaining)
            {
                Debug.Log("About to 8 sec warning");
                audioSource.clip = almostThere;
                audioSource.Play();
                Played8SecRemaining = true;
            }
        }

    }
    */
}

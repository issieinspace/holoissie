using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Timers;

public class GameManager : MonoBehaviour {

    public bool RunIntro = true;
    AudioClip completionCongrats = null;
    AudioClip partyMusic = null;
    AudioClip intro = null;

    AudioSource audioSource;

    public GameObject GamePrefab; // this is the prefab from which we will spawn games
    public GameObject CurrentGame; // this is the game in progress
    public GameObject[] exercises;
    public int currentExerciseIndex = 0;

    public Exercise CurrentExercise;
    public bool ExerciseStageInProgress;
    public bool ExerciseStageDone;
    public bool GameOver;
    public string StartScene;

    public GameObject Diagnostics;

    public string introClip = "OpeningDialogue_2_Remix";

    internal void OnGo()
    {
        throw new NotImplementedException();
    }


    // Use this for initialization
    void Start () {
        StartScene = SceneManager.GetActiveScene().name;

        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.gameObject.SetActive(false);
        
        // Setup some audio clips
        partyMusic = Resources.Load<AudioClip>("ISSIE Game Loop - Continuous Drums");
        intro = Resources.Load<AudioClip>(introClip);

        StartGame();
    }

    // Update is called once per frame
    void Update () {
        if (ExerciseStageInProgress)
        {
            RunExerciseStage();
        }
       
        if(Input.GetAxis("Cancel") != 0.0f)
        {
            RestartGame();
        }
      

    }

    private void StartGame()
    {
        // We will have multiple game stages and move one to the next
        if (RunIntro)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            
            TimersManager.SetTimer(this, audio.clip.length - 5, CommenceExerciseStage);
        }
        else
        {
            CommenceExerciseStage();
        }
      
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

            // Should move this elsewhere
            if(CurrentExercise.AchievementCount == 1 && !CurrentExercise.TriggeredFirstAchievementAttained)
            {
               // Debug.Log("Gamemanager sees you got an achievement");
                CurrentExercise.TriggeredFirstAchievementAttained = true;

                audioSource = GetComponent<AudioSource>();
                audioSource.clip = CurrentExercise.firstAchievementSound;
                audioSource.Play();
               // Debug.Log("Gamemanager played sound");
            }

            if (CurrentExercise.TimeLeft < 8 && !CurrentExercise.TriggeredAlmostDone)
            {
               // Debug.Log("Gamemanager sees you are almost done");
                CurrentExercise.TriggeredAlmostDone = true;

                audioSource = GetComponent<AudioSource>();
                audioSource.clip = CurrentExercise.almostDoneSound;
                audioSource.Play();
               // Debug.Log("Gamemanager played almost done sound");
            }

            OutputDiagnostics(CurrentExercise);
            // if all done, stage is done
        }
        else
        {
            ExerciseStageInProgress = false;
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

        currentExerciseIndex = 0;

        Debug.Log("Found " + exercises.Length + " exercises.");

        Array.Sort(exercises, delegate (GameObject ex1, GameObject ex2) {
            return ex1.GetComponent<Exercise>().Order.CompareTo(ex2.GetComponent<Exercise>().Order);
        });

        ExerciseStageInProgress = true;

        CurrentExercise = exercises[currentExerciseIndex].GetComponent<Exercise>();
        CurrentExercise.PlayerIsReady = true;
        Debug.Log("ExerciseStageStarted");   
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(StartScene);        
    }

    void HaveAParty()
    {
        GameObject[] alienFriends = GameObject.FindGameObjectsWithTag("Alien");
        Debug.Log("You got THIS many aliens rescued: " + alienFriends.Length);
        GameObject backgroundMusic = GameObject.Find("BackgroundMusic");

        audioSource = backgroundMusic.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = partyMusic;
        audioSource.loop = false;
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
    
}

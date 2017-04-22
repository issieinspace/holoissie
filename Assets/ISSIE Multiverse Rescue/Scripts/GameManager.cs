using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Timers;

public class GameManager : MonoBehaviour {

    public bool RunIntro = true;

    AudioClip partyMusic = null;

    AudioSource audioSource;

    public GameObject GamePrefab; // this is the prefab from which we will spawn games
    public GameObject CurrentGame; // this is the game in progress
    public GameObject[] exercises;
    public GameObject[] listeners;
    public int currentExerciseIndex = 0;

    public Exercise CurrentExercise;
    public bool ExerciseStageInProgress;
    public bool ExerciseStageDone;
    public bool GameOver;
    public string StartScene;

    public GameObject Diagnostics;
    public GameObject Score;
    public GameObject Credits;

    public string introClip = "OpeningDialogue_2_Remix";

   
    // Use this for initialization
    void Start () {
        //StartScene = SceneManager.GetActiveScene().name;

        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.gameObject.SetActive(false);
        
        // Setup some audio clips
        partyMusic = Resources.Load<AudioClip>("ISSIE Game Loop - Continuous Drums");

        Score = GameObject.Find("Score");
        Score.GetComponent<TextMesh>().text = "";

        Credits = GameObject.Find("Credits");
        Credits.SetActive(false);

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
                GameOver = true;
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

    public void HandleEvent(Hashtable args)
    {
        listeners = GameObject.FindGameObjectsWithTag("Triggerable");
        String methodName = (String)args["methodName"];
        Debug.Log("About to execute " + methodName);

        foreach (GameObject listener in listeners)
        {
            listener.SendMessage(methodName, args, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Sent " + methodName + " to " + listener.name + " with args " + args);
        }
    }

    void HaveAParty()
    {
        GameObject[] alienFriends = GameObject.FindGameObjectsWithTag("Alien");
        Debug.Log("You got THIS many aliens rescued: " + alienFriends.Length);
        GameObject backgroundMusic = GameObject.Find("AudioManager");

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

        GameObject.Find("Countdown").GetComponent<TextMesh>().text = "Game Over";
        Score.GetComponent<TextMesh>().text = "You rescued " + alienFriends.Length + " aliens!";
        
        
        TimersManager.SetTimer(this, 5, delegate { Credits.SetActive(true); Credits.AddComponent<CreditsAnimation>(); });
        

    }
    
}

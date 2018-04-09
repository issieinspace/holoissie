using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Timers;
using Prime31.MessageKit;
using Academy.HoloToolkit.Unity;

public class GameManager : MonoBehaviour {

    public bool RunIntro = true;
    public int TimeToWaitBeforeCommencingExercise = 10;

    public GameObject[] exercises;
    public int currentExerciseIndex = 0;

    public Exercise CurrentExercise;
    public bool ExerciseStageInProgress;
    public bool ExerciseStageDone;
    public bool GameOver;
    public string StartScene;

    public GameObject Diagnostics;
    public GameObject Score;
    public GameObject Credits;
    public SurfaceMeshesToPlanes hololensPlanes;
    [HideInInspector]
    public static float spacialFloorHeight = 0;
   
    // Use this for initialization
    void Start()
    {
        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.Object.SetActive(false);
        
        Score = GameObject.Find("Score");
        Score.GetComponent<TextMesh>().text = "";

        Credits = GameObject.Find("Credits");
        Credits.SetActive(false);
        
        StartGame();

#if !UNITY_EDITOR
        hololensPlanes.MakePlanesComplete += MoveWorldToSpacialFloor;
#endif
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("running the intro");
            StartCoroutine(VisibleAssetActivationDelay(TimeToWaitBeforeCommencingExercise));

            // Wait for the appropriate time before starting
            TimersManager.SetTimer(this, TimeToWaitBeforeCommencingExercise, CommenceExerciseStage);

            Debug.Log("sending the OnIntro message");
            // Send a message to play the intro
            MessageKit.post(MessageType.OnIntro);

            RunIntro = false;
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
                    Debug.Log("broadcasting Player ready within RunExercise");
                    BroadcastPlayerReady(CurrentExercise.name);
                    //CurrentExercise.PlayerIsReady = true;
                  
                    Debug.Log("Next exercise kicked off");
                }
            }


            OutputDiagnostics(CurrentExercise);
            // if all done, stage is done
        }
        else
        {
            ExerciseStageInProgress = false;
            if (!GameOver)
            {
                // Game is over now
                MessageKit.post(MessageType.OnGameOver);
                Credits.SetActive(true);
                GameOver = true;
            }
        }
    }

    private void BroadcastPlayerReady(string exerciseName)
    {
        Debug.Log("Broadcasting Player Ready for " + exerciseName);
        MessageKit<string>.post(MessageType.OnPlayerReady, exerciseName);
        Debug.Log("Done broadcasting Player Ready for " + exerciseName);
    }

    private void OutputDiagnostics(Exercise currentExercise)
    {
        Monitor monitor = Diagnostics.GetComponent<Monitor>();
        monitor.DisplayMessage(currentExercise.getStatus());
    }

    void CommenceExerciseStage()
    {
        // Collect the exercises
        exercises = CollectExercises();

        if (exercises.Length > 0)
        {
            ExerciseStageInProgress = true;

            CurrentExercise = exercises[currentExerciseIndex].GetComponent<Exercise>();

            Debug.Log("broadcasting Player ready within RunExercise");
            BroadcastPlayerReady(CurrentExercise.name);
            Debug.Log("ExerciseStageStarted");
        }
        
    }

    private GameObject[] CollectExercises()
    {
        GameObject[] collected = GameObject.FindGameObjectsWithTag("Exercise");
        currentExerciseIndex = 0;

        Debug.Log("Found " + exercises.Length + " exercises.");

        Array.Sort(exercises, delegate (GameObject ex1, GameObject ex2)
        {
            return ex1.GetComponent<Exercise>().Order.CompareTo(ex2.GetComponent<Exercise>().Order);
        });

        return collected;
    }

    public void RestartGame()
    {
        MessageKitManager.clearAllMessageTables();
        SceneManager.LoadScene(StartScene);        
    }



    public void MoveWorldToSpacialFloor(object source, EventArgs args)
    {

#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().path.Contains("Earth"))
        {
            transform.position = new Vector3(0, -1.8f, 0);
        }

        spacialFloorHeight = -1.8f;
#else
        transform.position = new Vector3(0, hololensPlanes.FloorYPosition, 0);
        spacialFloorHeight = hololensPlanes.FloorYPosition;
#endif
        MessageKit.post(MessageType.OnSpacialMappingComplete);
    }

    IEnumerator VisibleAssetActivationDelay(float time)
    {
        yield return new WaitForSeconds(time);
        MoveWorldToSpacialFloor(null, null);
    }
    
}

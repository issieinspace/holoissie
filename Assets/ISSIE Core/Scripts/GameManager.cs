using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Timers;
using Prime31.MessageKit;
using Academy.HoloToolkit.Unity;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    
    public bool RunIntro = true;
    public int TimeToWaitBeforeCommencingExercise = 10;
    public GameObject EventManagerGameObject = null;
    public IEventManager EventManager = null;

    public Exercise[] exercises;
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
    public static float spatialFloorHeight = 0;
    public int spatialMappingCompleteCount = 0;
   
    // Use this for initialization
    void Start()
    {
        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.Object.SetActive(false);

#if !UNITY_EDITOR
        hololensPlanes.MakePlanesComplete += MoveWorldToSpatialFloor;
#endif

        Score = GameObject.Find("Score");
        Score.GetComponent<TextMesh>().text = "";

        Credits = GameObject.Find("Credits");
        Credits.SetActive(false);

        if (EventManagerGameObject != null)
        {
            EventManager = EventManagerGameObject.GetComponent<IEventManager>();
            Debug.Log("Setting up EventManager that was specified");
            EventManager.Setup();
            Debug.Log("Done setting up EventManager that was specified");
        }
        
        StartGame();

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
            RestartGame(StartScene);
        }

        if (Input.GetAxis("Skip") != 0.0f)
        {
            SkipExercise();
        }
    }

    private void SkipExercise()
    {
        CurrentExercise.ExerciseComplete = true;
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
                    CurrentExercise = exercises[currentExerciseIndex];
                    Debug.Log("Broadcasting Player ready within RunExercise");
                    BroadcastPlayerReady(CurrentExercise.name);
                    Debug.Log("Next exercise kicked off");
                }
            }

            OutputDiagnostics(CurrentExercise);
            
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

            CurrentExercise = exercises[currentExerciseIndex];

            Debug.Log("Broadcasting Player ready within CommenceExercise");
            BroadcastPlayerReady(CurrentExercise.name);
            Debug.Log("ExerciseStageStarted");
        }
        
    }

    private Exercise[] CollectExercises()
    {
        GameObject workoutSelectionGameObject = GameObject.Find("WorkoutSelection");
        
        if (workoutSelectionGameObject != null)
        {
            if (workoutSelectionGameObject.GetComponent<WorkoutSelection>().UseWorkoutSelection)
                return workoutSelectionGameObject.GetComponent<WorkoutSelection>().SetupExercises();
        }

        return CollectExercisesByGameObject();
    }

    private Exercise[] CollectExercisesByGameObject()
    {
        GameObject[] found = GameObject.FindGameObjectsWithTag("Exercise");
        currentExerciseIndex = 0;
        List<Exercise> collected = new List<Exercise>();

        Debug.Log("Found " + found.Length + " exercises.");

        for (int i = 0; i < found.Length; i++)
        {
            Debug.Log("Setting up " + found[i].name);
            Exercise exercise = found[i].GetComponent<Exercise>();
            exercise.Setup();
            collected.Add(exercise);
        }

        collected.Sort((ex1, ex2) => ex1.Order.CompareTo(ex2.Order));

        return collected.ToArray();
    }

    public static void RestartGame(string scene)
    {
        MessageKitManager.clearAllMessageTables();
        SceneManager.LoadScene(scene);
    }
    
    public void MoveWorldToSpatialFloor(object source, EventArgs args)
    {
        spatialMappingCompleteCount++;
#if UNITY_EDITOR
        spatialMappingCompleteCount = 2;
#endif

        if (spatialMappingCompleteCount > 1)
        {
            Vector3 spatialFloorPosition = new Vector3(0, 0, 0);

#if UNITY_EDITOR
            spatialFloorHeight = -1.8f;

            if (SceneManager.GetActiveScene().path.Contains("Earth"))
            {
                spatialFloorPosition = new Vector3(0, spatialFloorHeight, 0);
            }

#else
        spatialFloorHeight = hololensPlanes.FloorYPosition;
        spatialFloorPosition = new Vector3(0, spatialFloorHeight, 0);
        Diagnostics.GetComponent<Monitor>().DisplayMessage("moved to floor at y = " + spatialFloorHeight);
#endif
            //transform.position = spatialFloorPosition;
            this.gameObject.AddComponent<FlyInAnimation>();
            this.gameObject.GetComponent<FlyInAnimation>().Setup(spatialFloorPosition);
            MessageKit.post(MessageType.OnSpatialMappingComplete);

        }

    }

    IEnumerator VisibleAssetActivationDelay(float time)
    {
        yield return new WaitForSeconds(time);
        MoveWorldToSpatialFloor(null, null);
    }
    
}

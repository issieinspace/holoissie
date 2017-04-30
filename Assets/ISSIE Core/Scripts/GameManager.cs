using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Timers;
using Prime31.MessageKit;
using Academy.HoloToolkit.Unity;

public class GameManager : MonoBehaviour {

    public bool RunIntro = true;
    public int TimeToWaitBeforeCommencingExercise = 0;

    public GameObject[] exercises;
    private GameObject[] listeners;
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
    void Start () {

        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.Object.SetActive(false);
        
        Score = GameObject.Find("Score");
        //Score.GetComponent<TextMesh>().text = "";

        Credits = GameObject.Find("Credits");
        //Credits.SetActive(false);

        listeners = GameObject.FindGameObjectsWithTag("Triggerable");


        StartGame();
        hololensPlanes.MakePlanesComplete += MoveWorldToSpacialFloor;
        //MessageKit<string>.addObserver(MessageType.OnReady, (name) => MoveWorldToSpacialFloor());
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
            MoveWorldToSpacialFloor(null, null);
            // Send a message to play the intro
            //Hashtable args = new Hashtable();
            //args.Add("methodName", "OnIntro");
            //HandleEvent(args);
            MessageKit.post(MessageType.OnIntro);
           
            // Wait for the appropriate time before starting
            TimersManager.SetTimer(this, TimeToWaitBeforeCommencingExercise, CommenceExerciseStage);
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
                    CurrentExercise.PlayerIsReady = true;
                  
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
                //Hashtable args = new Hashtable();
                //args.Add("methodName", "OnGameOver");
                //HandleEvent(args);
                MessageKit.post(MessageType.OnGameOver);
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
        MessageKitManager.clearAllMessageTables();
        SceneManager.LoadScene(StartScene);        
    }

    public void HandleEvent(Hashtable args)
    {
        String methodName = (String)args["methodName"];
        Debug.Log("About to execute " + methodName);

        foreach (GameObject listener in listeners)
        {
            listener.SendMessage(methodName, args, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Sent " + methodName + " to " + listener.name + " with args " + args);
        }
    }

    public void MoveWorldToSpacialFloor(object source, EventArgs args)
    {
        
#if UNITY_EDITOR
        transform.position = new Vector3(0, -1.8f, 0);
        spacialFloorHeight = -1.8f;
#else
        transform.position = new Vector3(0, hololensPlanes.FloorYPosition, 0);
        spacialFloorHeight = hololensPlanes.FloorYPosition;
#endif
        MessageKit.post(MessageType.OnSpacialMappingComplete);
    }
    
}

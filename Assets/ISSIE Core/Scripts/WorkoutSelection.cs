using System.Collections.Generic;
using UnityEngine;


public class WorkoutSelection : MonoBehaviour {
    //Responsible for loading the list of exercises in a game and recording the choice of Workout

    public string _gameFile;
    public WorkoutCatalog _workoutCatalog;
    public string _chosenWorkout;
    public bool _useWorkoutSelection;

    public Dictionary<string, GameObject> _exercisePrefabs = new Dictionary<string, GameObject>();



    public void Start()
    {
        string jsonText = LoadResourceTextFile(_gameFile);
        _workoutCatalog = JsonUtility.FromJson<WorkoutCatalog>(jsonText);
    }

    private string LoadResourceTextFile(string GameFile)
    {
        Debug.Log("Loading workouts from " + GameFile);
        TextAsset targetFile = Resources.Load<TextAsset>(GameFile);
        return targetFile.text;
    }

    public Exercise[] SetupExercises()
    {
        Exercise[] exerciseGameObjects = null;

        WorkoutDefinition workoutDef = _workoutCatalog.Workouts.Find(delegate(WorkoutDefinition w) { return w.Name == _chosenWorkout; });

        foreach(ExerciseDefinition exerciseDef in workoutDef.Exercises)
        {
            //GameObject.Instantiate<GameObject>()
        }

        return exerciseGameObjects;
    }
}

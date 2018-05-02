using UnityEngine;

public class WorkoutSelection : MonoBehaviour {
    //Responsible for loading the list of exercises in a game and recording the choice of Workout

    public string GameFile;
    public WorkoutCatalog WorkoutCatalog;
    public string ChosenWorkout;

    public void Start()
    {
        string jsonText = LoadResourceTextFile(GameFile);
        WorkoutCatalog = JsonUtility.FromJson<WorkoutCatalog>(jsonText);
    }

    private string LoadResourceTextFile(string GameFile)
    {
        Debug.Log("Loading workouts from " + GameFile);
        TextAsset targetFile = Resources.Load<TextAsset>(GameFile);
        return targetFile.text;
    }
}

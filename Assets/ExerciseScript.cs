using UnityEngine;
using System.Collections;

public class ExerciseScript : MonoBehaviour {

    public GameObject ExerciseMovePrefab; // we make a new exercise prefab each time a move completes
    public int movesToUnlockAliens; // defines how many moves to unlock an alien
    public bool isExerciseDone = false; 
    public float secondsForExercise; // how long you have for the exercise
    public float timeLeft;
    public int countOfMovesCompleted;
    public bool isFirstAchievementUnlocked = false;
    
    ExerciseMoveScript ExerciseMove; // the current exercise move

	// Use this for initialization
	void Start () {
        timeLeft = secondsForExercise;
        ExerciseMove = Instantiate(ExerciseMovePrefab).GetComponent<ExerciseMoveScript>();
    }
	
	
	void Update () {
       
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            isExerciseDone = true;
        }

        
    }
}

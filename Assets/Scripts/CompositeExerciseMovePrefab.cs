using UnityEngine;
using System.Collections;
using System;

public class CompositeExerciseMovePrefab : ExerciseMove {

    public GameObject[] MovePrefabs;
    int currentExerciseIndex;
    public ExerciseMove CurrentMove;

    // Use this for initialization
    void Start () {
        currentExerciseIndex = 0;
        CurrentMove = instantiateMove(currentExerciseIndex);
        Debug.Log("There are " + MovePrefabs.Length + " moves in this composite. Setup the first one:" + CurrentMove.moveName);
    }

    private ExerciseMove instantiateMove(int index)
    {
        return GameObject.Instantiate(MovePrefabs[index]).GetComponent<ExerciseMove>();
    }

    // Update is called once per frame
    void Update () {
        CurrentMove.Run();

        if(CurrentMove.Complete)
        {
            Debug.Log("Advanced to next Move" + CurrentMove.moveName);

            currentExerciseIndex++;
            CurrentMove = instantiateMove(currentExerciseIndex);
        }

        if(currentExerciseIndex == MovePrefabs.Length)
        {
            Debug.Log("Completed Composite Move");

            this.Complete = true;
        }
	}

    ExerciseMove getCurrentMove()
    {
        return CurrentMove;
    }

    public override string getMoveName()
    {
        return CurrentMove.getMoveName();
    }

    public override float getXDisplacement()
    {
       // Debug.Log("Behold... the getXDisplacement() doth delegate" + CurrentMove.getXDisplacement());
        return CurrentMove.getXDisplacement();
    }

    public override float getYDisplacement()
    {
        return CurrentMove.getYDisplacement();
    }

    public override float getZDisplacement()
    {
        return CurrentMove.getZDisplacement();
    }

    public override bool getDisplacementAchieved()
    {
        return CurrentMove.getDisplacementAchieved();
    }

}

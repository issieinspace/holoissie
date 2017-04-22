using UnityEngine;
using System.Collections;

public class CEVISExerciseMove : ExerciseMove {

    protected override bool checkForDisplacement()
    {

        return (Input.GetAxis("Jump") != 0.0f);
    }

    protected override bool checkForCompletion()
    {
        return (Input.GetAxis("Jump") == 0.0f);
    }
}

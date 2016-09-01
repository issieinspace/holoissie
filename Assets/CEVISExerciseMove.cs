using UnityEngine;
using System.Collections;

public class CEVISExerciseMove : ExerciseMove {

    protected override bool checkForDisplacement()
    {
        Debug.Log("Checking for displacement will return true");
        return true;
    }

    protected override bool checkForCompletion()
    {
        Debug.Log("Checking for completion checks for Jump");

        float jumpButton = Input.GetAxis("Jump");

        if (jumpButton != 0.0f)
        {
            Debug.Log("jumpButton " + jumpButton);
            return true;
        }
         else
        {
            return false;

        }
    }
}

using UnityEngine;
using System.Collections;

public class CEVISExerciseMove : ExerciseMove {

    CycleReadout CycleReadout;

    protected override void setup()
    {
        base.setup();
        //GameObject.Find("");
    }

    protected override bool checkForDisplacement()
    {

        return (Input.GetAxis("Jump") != 0.0f);
    }

    protected override bool checkForCompletion()
    {
        return (Input.GetAxis("Jump") == 0.0f);
    }
}

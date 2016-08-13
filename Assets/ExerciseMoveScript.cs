using UnityEngine;
using System.Collections;
using System;

public class ExerciseMoveScript : MonoBehaviour {

    public float xDisplacement;
    public float yDisplacement;
    public float zDisplacement;

    public bool isComplete = false;
    public bool isDisplacedAchieved = false;
    public GameObject transporterControl;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    // Has displacement been achieved?
        if(checkDisplacementAcheived())
        {
            isDisplacedAchieved = true;
        }

        // Has return been achieved?
        if (isDisplacedAchieved)
        {
            if (checkReturnAcheived())
            {
                isComplete = true;
            }
        }
	}

    private bool checkReturnAcheived()
    {
        return false;
    }

    private bool checkDisplacementAcheived()
    {
       return false;
    }
}

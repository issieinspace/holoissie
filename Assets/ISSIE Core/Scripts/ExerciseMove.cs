using UnityEngine;
using System.Collections;
using System;

public class ExerciseMove : MonoBehaviour {

    public string moveName;

    internal void Run()
    {
        Update();
    }

    // The x,y,z distances you are required to move 
    public float targetXDisplacement;
    public float targetYDisplacement;
    public float targetZDisplacement;
    public float wiggleRoom = .1f; // the amount of room it is permissible to be off when you return from displacement

    public Vector3 originalPosition;
 
    public bool Complete = false;
    public bool DisplacementAchieved = false;
    public float TimeElapsed = 0f;

    public float xDisplacement;
    public float yDisplacement;
    public float zDisplacement;

    public bool TriggeredDisplacementAchieved { get; internal set; }

    void Start () {
        Debug.Log("Exercise move " + getMoveName() + " is setup");
    }

    void Update () {
        if (!DisplacementAchieved)
        {
            //test for displacement
            DisplacementAchieved = checkForDisplacement();
        }
        else if (!Complete)
        {
            //test for return from displacement
            Complete = checkForCompletion();
            // Advance the clock
            TimeElapsed += Time.deltaTime;
        }
    }

    protected virtual bool checkForDisplacement()
    {
        // Get the position
        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 displacement = currentPosition - originalPosition;

       // Debug.Log("Testing for displacement" + displacement.ToString());
        xDisplacement = displacement.x;
        yDisplacement = displacement.y;
        zDisplacement = displacement.z;

        // Test for movement
        return checkDisplacementAxis(displacement.x, targetXDisplacement)
                && checkDisplacementAxis(displacement.y, targetYDisplacement)
                && checkDisplacementAxis(displacement.z, targetZDisplacement);
  
    }

    private bool checkDisplacementAxis(float displacement, float targetDisplacement)
    {
        bool val = false;
         
        if (targetDisplacement < 0)
        {
            val = displacement <= targetDisplacement;
            //Debug.Log("Target is a negative number so displacement " + displacement + " <= target " + targetDisplacement + " is " + val);
        }
        else if (targetDisplacement > 0)
        {
            val = displacement >= targetDisplacement;
            //Debug.Log("Target is a positive number so displacement " + displacement + " >= target " + targetDisplacement + " is " + val);
        }
        else
        {
            //target displacement == 0
            val = true;
        }

        return val;
    }

    protected virtual bool checkForCompletion()
    {
        // Test that we have returned from displacement
        // Get the position
        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 displacement = currentPosition - originalPosition;

        //Debug.Log("Testing for return" + displacement.ToString());
        xDisplacement = displacement.x;
        yDisplacement = displacement.y;
        zDisplacement = displacement.z;

        return checkCompletionAxis(displacement.x, targetXDisplacement, wiggleRoom)
                && checkCompletionAxis(displacement.y, targetYDisplacement, wiggleRoom)
                && checkCompletionAxis(displacement.z, targetZDisplacement, wiggleRoom);
                

        /*Debug.Log(" displacement " + yDisplacement + " < wiggleroom " + wiggleRoom + " is " + (displacement.x < wiggleRoom));
        // Test for movement
        return ((displacement.x < wiggleRoom || targetXDisplacement == 0)
                && (displacement.y < wiggleRoom || targetYDisplacement == 0)
                && (displacement.z < wiggleRoom || targetZDisplacement == 0));
        */
    }

    public virtual string getMoveName()
    {
        return moveName;
    }

    public virtual float getXDisplacement()
    {
        return xDisplacement;
    }

    public virtual float getYDisplacement()
    {
        return yDisplacement;
    }

    public virtual float getZDisplacement()
    {
        return zDisplacement;
    }

    public virtual float getTargetYDisplacement()
    {
        return targetYDisplacement;
    }

    public virtual float getTargetZDisplacement()
    {
        return targetZDisplacement;
    }

    public virtual float getTargetXDisplacement()
    {
        return targetXDisplacement;
    }


    public virtual bool getDisplacementAchieved()
    {
        return DisplacementAchieved;
    }

 
    private bool checkCompletionAxis(float displacement, float targetDisplacement, float wiggleRoom)
    {

        if(targetDisplacement == 0)
        {
            return true;
        }

        bool val = false;

        // Wiggle room allows us to say how far off a return from displacement we are allowed to get and still be "good enough"
        // 

        if (wiggleRoom <= 0)
        {
            val = displacement > wiggleRoom;
            //Debug.Log("Wiggle is a negative number so displacement " + displacement + " <= target " + wiggleRoom + " is " + val);
        }
        else if (wiggleRoom > 0)
        {
            val = displacement < wiggleRoom;
           // Debug.Log("Wiggle is a positive number so displacement " + displacement + " >= target " + wiggleRoom + " is " + val);
        }
        else
        {
            //perfect return or we don't care == 0
            val = true;
        }

        return val;
    }
}

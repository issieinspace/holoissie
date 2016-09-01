using UnityEngine;
using System.Collections;

public class ExerciseMove : MonoBehaviour {

    AudioClip bleep = null;

    public string moveName;

    // target displacements must be calculated relative to the height of the user. This is especially true for moves that require negative y displacements
    // this option allows you to calculate a displacement that is fixed off of the distance between the transporter and the user
    //public bool CalculateDisplacementFromTransporter;
    public GameObject TransporterControl;

    // The x,y,z distances you are required to move 
    public float targetXDisplacement;
    public float targetYDisplacement;
    public float targetZDisplacement;
    public float wiggleRoom = .1f; // the amount of room it is permissible to be off when you return from displacement

    Vector3 originalPosition;
 
    public bool Complete = false;
    public bool DisplacementAchieved = false;
    public float TimeElapsed = 0f;

    public float xDisplacement;
    public float yDisplacement;
    public float zDisplacement;

    public bool TriggeredDisplacementAchieved { get; internal set; }

    void Start () {
       bleep = Resources.Load<AudioClip>("Computer04");
       // get the initial position of the player
       originalPosition = Camera.main.transform.position;
       // Exercise is required to set a transporter on us 
       Debug.Log("Exercise move is setup");
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
        Vector3 displacement = originalPosition - currentPosition;

        //Debug.Log("Testing for displacement" + displacement.ToString());
        xDisplacement = displacement.x;
        yDisplacement = displacement.y;
        zDisplacement = displacement.z;

        // Test for movement
        return ((displacement.x >= targetXDisplacement || targetXDisplacement == 0)
                && (displacement.y >= targetYDisplacement || targetYDisplacement == 0)
                && (displacement.z >= targetZDisplacement || targetZDisplacement == 0));
  
    }

    protected virtual bool checkForCompletion()
    {
        // Test that we have returned from displacement
        // Get the position
        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 displacement = originalPosition - currentPosition;

        //Debug.Log("Testing for return" + displacement.ToString());
        xDisplacement = displacement.x;
        yDisplacement = displacement.y;
        zDisplacement = displacement.z;
        
        // Test for movement
        return ((displacement.x < wiggleRoom || targetXDisplacement == 0)
                && (displacement.y < wiggleRoom || targetYDisplacement == 0)
                && (displacement.z < wiggleRoom || targetZDisplacement == 0));

    }
}

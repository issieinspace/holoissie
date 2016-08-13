using UnityEngine;
using System.Collections;
using System;

public class GameScript : MonoBehaviour {
    public System.Collections.Generic.List<GameObject> ExerciseStations;
    public GameObject TransporterControlBlue;
    public GameObject TransporterControlGreen;
    public GameObject TransporterControlRed;
    public GameObject TransporterControlPurple;
    

    public GameObject SquatsPrefab;
    
    // Use this for initialization
    void Start () {
        // add all the transporters to the ExerciseStations collection
        // first create a TransportControl for each transporter and add
        GameObject squats = Instantiate(SquatsPrefab);
        GameObject blue = SetupExerciseStation(TransporterControlBlue);
        ExerciseStations.Add(blue);

    }

  
    private GameObject SetupExerciseStation(GameObject transporterControl)
    {
        // Create a TransportControl object and add as a child to the Transporter
        
        return transporterControl;
    }

    private GameObject CreateExercise(string v)
    {
        return null;
    }

    // Update is called once per frame
    void Update () {
	
	}
}

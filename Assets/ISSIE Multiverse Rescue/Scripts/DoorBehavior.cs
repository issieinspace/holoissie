using UnityEngine;
using System.Collections;
using System;
using Timers;

public class DoorBehavior : MonoBehaviour {

    GameObject door;
	// Use this for initialization
	void Start () {
        door = this.gameObject;
	}
	



    internal void TriggerReady()
    {
        Debug.Log("Triggering the door");

            door.GetComponent<Animation>()["Door_01"].speed = 1;

            door.GetComponent<Animation>()["Door_01"].time = 0;

            door.GetComponent<Animation>().Play();

            TimersManager.SetTimer(this, 2f, CloseDoor);
        
    }

    void CloseDoor()
 	{

        door.GetComponent<Animation>()["Door_01"].speed = -1;

        door.GetComponent<Animation>()["Door_01"].time = door.GetComponent< Animation > ()["Door_01"].length;

        door.GetComponent<Animation>().Play();

   }
}

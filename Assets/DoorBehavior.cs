using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour {

    GameObject door;
	// Use this for initialization
	void Start () {
        door = this.gameObject;
	}
	

    public void TriggerStart()
    {
        if (!door.GetComponent< Animation > ().isPlaying)
        {
            door.GetComponent< Animation > ()["Door_01"].speed = 1;
           
            door.GetComponent< Animation > ()["Door_01"].time = 0;
           
            door.GetComponent< Animation > ().Play();
        }
    }
	
	/*if (GUI.Button(Rect(Screen.width / 2 + 25, 50, 100, 30),"Close"))
	{
		if (!door1.GetComponent.<Animation>().isPlaying && !door2.GetComponent.<Animation>().isPlaying)
		{
			door1.GetComponent.<Animation>()["Door_01"].speed = -1;
			door2.GetComponent.<Animation>()["Door_02"].speed = -1;
			door1.GetComponent.<Animation>()["Door_01"].time = door1.GetComponent.<Animation>()["Door_01"].length;
			door2.GetComponent.<Animation>()["Door_02"].time = door2.GetComponent.<Animation>()["Door_02"].length;
		
			door1.GetComponent.<Animation>().Play();
    door2.GetComponent.<Animation>().Play();

    controlPanelScreen.GetComponent.<Renderer>().material.mainTexture = controlPanelLocked;
		}
	}
    }*/
}

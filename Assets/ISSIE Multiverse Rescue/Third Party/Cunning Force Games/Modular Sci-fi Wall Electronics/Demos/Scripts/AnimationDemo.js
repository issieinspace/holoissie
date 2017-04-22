#pragma strict

var door1 : GameObject;
var door2 : GameObject;
var controlPanelScreen : GameObject;
var controlPanelLocked : Texture;
var controlPanelUnlocked : Texture;

private var opened : boolean = false;

function OnGUI()
{
	if (GUI.Button(Rect(Screen.width / 2 - 125, 50, 100, 30),"Open"))
	{
		if (!door1.GetComponent.<Animation>().isPlaying && !door2.GetComponent.<Animation>().isPlaying)
		{
			door1.GetComponent.<Animation>()["Door_01"].speed = 1;
			door2.GetComponent.<Animation>()["Door_02"].speed = 1;
			door1.GetComponent.<Animation>()["Door_01"].time = 0;
			door2.GetComponent.<Animation>()["Door_02"].time = 0;
		
			door1.GetComponent.<Animation>().Play();
			door2.GetComponent.<Animation>().Play();
			
			controlPanelScreen.GetComponent.<Renderer>().material.mainTexture = controlPanelUnlocked;
		}
	}
	
	if (GUI.Button(Rect(Screen.width / 2 + 25, 50, 100, 30),"Close"))
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
}
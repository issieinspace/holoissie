#pragma strict
/* This script controls the amount of flickering for the holographic screens */

var minOnDelay : float = 0.5;		// Minimum time in seconds to stay on
var maxOnDelay : float = 5.0;		// Maximum time in seconds to stay on
var minOffDelay : float = 0.01;		// Minimum time in seconds to stay off
var maxOffDelay : float = 0.02;		// Maximum time in seconds to stay off
var minAlpha : float = 0.6;			// Minimum alpha of material
var maxAlpha : float = 0.7;			// Maximum alpha of material

private var screenContent : GameObject;

function Start ()
{
	screenContent = transform.Find("Screen_Content").gameObject;	// Find "Screen_Content" object
	
	while (true)
	{
		 screenContent.active = true;		// Turn on screen content
		 screenContent.GetComponent.<Renderer>().material.color.a = Random.Range(minAlpha, maxAlpha);	// Set alpha of screen content
		 yield WaitForSeconds (Random.Range(minOnDelay, maxOnDelay));	// Wait a random time between minOnDelay and maxOnDelay
		 screenContent.active = false;	// Turn off screen content
		 yield WaitForSeconds (Random.Range(minOffDelay, maxOffDelay));		// Wait a random time between minOffDelay and maxOffDelay
		 
		 yield;
	}
}
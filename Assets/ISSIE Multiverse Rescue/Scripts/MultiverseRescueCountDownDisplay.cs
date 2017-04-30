using UnityEngine;
using System.Collections;
using System;

public class MultiverseRescueCountDownDisplay : MonoBehaviour {

    uint Tick;
    string[] ReadySetGo = new string[6] { "", "GO", "Set", "Set", "Ready", "Ready"};

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = "";

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void TriggerTick(uint countDown)
    {
        GetComponent<TextMesh>().text = ReadySetGo[countDown];
        

    }
}

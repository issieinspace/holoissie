using UnityEngine;
using System.Collections;
using System;
using Prime31.MessageKit;


public class MultiverseRescueCountDownDisplay : MonoBehaviour {

    string[] ReadySetGo = new string[6] { "", "GO", "Set", "Set", "Ready", "Ready"};

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = "";
        MessageKit<uint>.addObserver(MessageType.OnTick, (countDown) => OnTick(countDown));
    }
    
    internal void OnTick(uint countDown)
    {
       GetComponent<TextMesh>().text = ReadySetGo[countDown];
    }
}

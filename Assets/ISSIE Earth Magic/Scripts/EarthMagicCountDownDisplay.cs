using UnityEngine;
using System.Collections;
using System;
using Prime31.MessageKit;

public class EarthMagicCountDownDisplay : MonoBehaviour {

    uint Tick;
    string[] ReadySetGo = new string[6] { "", "GO", "Set", "Set", "Ready", "Ready"};

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = "";
        MessageKit<uint>.addObserver(MessageType.OnTick, (countDown) => OnTick(countDown));

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTick(uint countDown)
    {
        GetComponent<TextMesh>().text = ReadySetGo[countDown];
        
    }
}

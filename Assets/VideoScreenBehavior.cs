using Prime31.MessageKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoScreenBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MessageKit.addObserver(MessageType.OnGameOver, Disappear);
    }
	
	// Update is called once per frame
	void Disappear () {
        this.gameObject.SetActive(false);
	}
}

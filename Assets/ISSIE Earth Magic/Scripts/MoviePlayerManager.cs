using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prime31.MessageKit;

using UnityEngine;
using UnityEngine.Video;

public class MoviePlayerManager : MonoBehaviour {


    private VideoPlayer videoPlayer;
    private CanvasRenderer[] renderers;
    
    private bool isPlaying;
    private VideoClip _videoPlayerClip;

    // Use this for initialization
	void Awake()
	{
        // Get an instance of the Video Clip
	    videoPlayer = this.GetComponentInChildren<VideoPlayer>();
	    renderers = this.GetComponentsInChildren<CanvasRenderer>();
            
        // Turn off until we need it
        MessageKit<string>.addObserver(MessageType.OnReady, OnReady);
        MessageKit.addObserver(MessageType.OnTimeDone, OnTimeDone);
    }

    private void OnReady(string exerciseName)
    {
        Debug.Log("Video Player is ready for " + exerciseName);
        // Find the related Video and make the canvas visible
        UnityEngine.UI.Text text = this.GetComponentInChildren<UnityEngine.UI.Text>();
        text.text = exerciseName;

        _videoPlayerClip = Resources.Load<VideoClip>(exerciseName);
        videoPlayer.isLooping = true;
        TurnOn();
        videoPlayer.clip = _videoPlayerClip;
        
        videoPlayer.Play();

        isPlaying = true;
        
    }

    private void OnTimeDone()
    {
        TurnOff();
    }


    // Update is called once per frame
    void Update () {
	    if (isPlaying)
	    {
	        if (!videoPlayer.isPlaying)
	        {
	            TurnOff();
	            isPlaying = false;
	        }
	    }
	}
    //TODO: There's got to be a better way to do this.
    void TurnOff()
    {
        gameObject.SetActive(false);
        //foreach (CanvasRenderer r in renderers)
        //{
        //    r.SetAlpha(0f);
        //}
        Resources.UnloadAsset(_videoPlayerClip);
    }

    void TurnOn()
    {
        gameObject.SetActive(true);
        //foreach (CanvasRenderer r in renderers)
        //{
        //    r.SetAlpha(1f);
        //}
    }
}

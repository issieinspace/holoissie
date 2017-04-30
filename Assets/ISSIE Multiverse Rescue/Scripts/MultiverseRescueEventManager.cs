﻿using UnityEngine;
using System.Collections;
using Timers;
using Prime31.MessageKit;

public class MultiverseRescueEventManager : MonoBehaviour {

    AudioSource backgroundMusic = null;
    AudioSource audioSource = null;

    // AudioClips to use for marking progress
    public string achievementSoundName = "EtherealAccent";
    public string timeDoneSoundName = "SynthZap";
    public string firstAchievementName = "NICEWORK1-Remix";
    public string almostDoneName = "YourAlmostThere1-Remix";
    public string introClipName = "OpeningDialogue_2_Remix";
    public string partyMusicName = "ISSIE Game Loop - Continuous Drums";
    public string backgroundMusicName = "ISSIE open";

    AudioClip achievementSound = null;
    AudioClip almostDoneSound = null;
    AudioClip firstAchievementSound = null;
    AudioClip intro = null;
    AudioClip partyMusic = null;
   
    void Start ()
    {
        achievementSound = Resources.Load<AudioClip>(achievementSoundName);
        almostDoneSound = Resources.Load<AudioClip>(almostDoneName);
        firstAchievementSound = Resources.Load<AudioClip>(firstAchievementName);
        intro = Resources.Load<AudioClip>(introClipName);
        partyMusic = Resources.Load<AudioClip>(partyMusicName);

        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        audioSource = audioSources[1];
        audioSource.loop = false;

        backgroundMusic = audioSources[2];
        backgroundMusic.loop = true;

        MessageKit.addObserver(MessageType.OnIntro, OnIntro);
        MessageKit.addObserver(MessageType.OnFirstAchievement, OnFirstAchievement);
        MessageKit.addObserver(MessageType.OnGameOver, OnGameOver);
        MessageKit.addObserver(MessageType.OnAlmostDone, OnAlmostDone);
    }

    public void OnIntro()
    {
        audioSource.clip = intro;
        audioSource.Play();

    }

    public void OnFirstAchievement()
    {
        audioSource.clip = firstAchievementSound;
        audioSource.Play();
    }

    public void OnGameOver()
    {
        HaveAParty();
    }

    public void OnAlmostDone()
    {
        audioSource.clip = almostDoneSound;
        audioSource.Play();
    }

    void playClip(AudioClip clip)
    {

    }

    void HaveAParty()
    {
        GameObject[] alienFriends = GameObject.FindGameObjectsWithTag("Alien");
        Debug.Log("You got THIS many aliens rescued: " + alienFriends.Length);

        backgroundMusic.Stop();
        backgroundMusic.clip = partyMusic;
        backgroundMusic.loop = false;
        backgroundMusic.Play();

        foreach (GameObject alien in alienFriends)
        {
            if (!alien.GetComponent<AlienBehaviour>().isdropped)
            {
                alien.GetComponent<AlienBehaviour>().OnDrop();
            }

            alien.GetComponent<AlienBehaviour>().DanceParty();
        }

        GameObject.Find("Countdown").GetComponent<TextMesh>().text = "Game Over";
        GameObject.Find("Score").GetComponent<TextMesh>().text = "You rescued " + alienFriends.Length + " aliens!";
        GameObject Credits = GameObject.Find("Credits");

        //TimersManager.SetTimer(this, 5, delegate { Credits.SetActive(true); Credits.AddComponent<CreditsAnimation>(); });


    }
}

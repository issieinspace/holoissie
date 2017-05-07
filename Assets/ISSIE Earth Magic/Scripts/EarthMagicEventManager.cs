using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Timers;
using Prime31.MessageKit;

public class EarthMagicEventManager : MonoBehaviour
{

    AudioSource audioSource = null;

    // AudioClips to use for marking progress
    public string startSoundName = "EtherealAccent";
    public string timeDoneSoundName = "SynthZap";
    public string firstAchievementName = "NICEWORK1-Remix";
    public string almostDoneName = "YourAlmostThere1-Remix";
    public string introClipName = "OpeningDialogue_2_Remix";
    public string partyMusicName = "ISSIE Game Loop - Continuous Drums";
    public string badoopSoundName = "Computer04";

    // State related info
    AudioClip startSound = null;
    AudioClip almostDoneSound = null;
    AudioClip firstAchievement = null;
    AudioClip intro = null;
    AudioClip partyMusic = null;
    AudioClip timeDoneSound = null;
    AudioClip bleep = null;

    // Use this for initialization
    void Start()
    {
        // Spatial Mapping
        //SpatialMapping.Instance.Object.SetActive(false);
        
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = false;

        startSound = Resources.Load<AudioClip>(startSoundName);
        almostDoneSound = Resources.Load<AudioClip>(almostDoneName);
        firstAchievement = Resources.Load<AudioClip>(firstAchievementName);
        intro = Resources.Load<AudioClip>(introClipName);
        partyMusic = Resources.Load<AudioClip>(partyMusicName);
        timeDoneSound = Resources.Load<AudioClip>(timeDoneSoundName);
        bleep = Resources.Load<AudioClip>(badoopSoundName);

        MessageKit.addObserver(MessageType.OnIntro, OnIntro);
        MessageKit.addObserver(MessageType.OnFirstAchievement, OnFirstAchievement);
        MessageKit.addObserver(MessageType.OnGameOver, OnGameOver);
        MessageKit.addObserver(MessageType.OnAlmostDone, OnAlmostDone);

        MessageKit<string>.addObserver(MessageType.OnReady, (name) => OnReady(name));
        MessageKit.addObserver(MessageType.OnDisplacementAchieved, OnDisplacementAchieved);
        MessageKit.addObserver(MessageType.OnMoveComplete, OnMoveComplete);
        MessageKit.addObserver(MessageType.OnAchievement, OnAchievement);
        MessageKit.addObserver(MessageType.OnStart, OnStart);
        MessageKit.addObserver(MessageType.OnTimeDone, OnTimeDone);
       
    }

    public void OnIntro()
    {
        audioSource.clip = intro;
        audioSource.Play();

    }

    public void OnReady(string name)
    {
        //intentionally blank
        Debug.Log("EMEvtMgr is OnReady");
    }

    public void OnDisplacementAchieved()
    {
        audioSource.clip = bleep;
        audioSource.Play(); 
    }

    public void OnMoveComplete()
    {
        audioSource.clip = bleep;
        audioSource.Play();
    }

    public void OnAchievement()
    {
        //intentionally blank
    }

    public void OnStart()
    {
        audioSource.clip = startSound;
        audioSource.Play();
    }

    public void OnTimeDone()
    {

    }

    public void OnFirstAchievement()
    {
        audioSource.clip = firstAchievement;
        audioSource.Play();
    }

    // This should probably go somewhere else... Like on the countdown and score objects
    public void OnGameOver()
    {
        GameObject.Find("Countdown").GetComponent<TextMesh>().text = "Game Over";
        GameObject.Find("Score").GetComponent<TextMesh>().text = "You've restored the Earth";
    }

    public void OnAlmostDone()
    {
        audioSource.clip = almostDoneSound;
        audioSource.Play();
    }
   
}

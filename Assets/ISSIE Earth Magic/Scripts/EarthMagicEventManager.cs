using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Timers;

public class EarthMagicEventManager : MonoBehaviour
{

    AudioSource audioSource = null;

    // AudioClips to use for marking progress
    public string achievementSoundName = "EtherealAccent";
    public string timeDoneSoundName = "SynthZap";
    public string firstAchievementName = "NICEWORK1-Remix";
    public string almostDoneName = "YourAlmostThere1-Remix";
    public string introClipName = "OpeningDialogue_2_Remix";
    public string partyMusicName = "ISSIE Game Loop - Continuous Drums";

    // State related info
    AudioClip achievementSound = null;
    AudioClip almostDoneSound = null;
    AudioClip firstAchievement = null;
    AudioClip intro = null;
    AudioClip partyMusic = null;
    AudioClip timeDoneSound = null;

    // Use this for initialization
    void Start()
    {
        // Spatial Mapping
        SpatialMapping.Instance.Object.SetActive(false);
        
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = false;

        achievementSound = Resources.Load<AudioClip>(achievementSoundName);
        almostDoneSound = Resources.Load<AudioClip>(almostDoneName);
        firstAchievement = Resources.Load<AudioClip>(firstAchievementName);
        intro = Resources.Load<AudioClip>(introClipName);
        partyMusic = Resources.Load<AudioClip>(partyMusicName);
        timeDoneSound = Resources.Load<AudioClip>(timeDoneSoundName);

    }

    public void OnIntro()
    {
        audioSource.clip = intro;
        audioSource.Play();

    }

    public void OnFirstAchievement()
    {
        audioSource.clip = firstAchievement;
        audioSource.Play();
    }

    public void OnGameOver(Hashtable args)
    {
        HaveAParty();
    }

    public void OnAlmostDone(Hashtable args)
    {

    }

    void playClip(AudioClip clip)
    {

    }

    void HaveAParty()
    {
        GameObject[] alienFriends = GameObject.FindGameObjectsWithTag("Alien");
        Debug.Log("You got THIS many aliens rescued: " + alienFriends.Length);

        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = partyMusic;
        audioSource.loop = false;
        audioSource.Play();

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

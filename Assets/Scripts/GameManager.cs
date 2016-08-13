using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Transporter;
    public bool RunIntro = true;
    public bool Played8SecRemaining = false;
    public bool PlayedFirstAchievement = false;
    AudioClip almostThere = null;
    AudioClip firstAchievement = null;
    AudioClip completionCongrats = null;

    public GameObject GamePrefab; // this is the prefab from which we will spawn games
    public GameObject CurrentGame; // this is the game in progress


    // Use this for initialization
    void Start () {
        CurrentGame = Instantiate(GamePrefab);

        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.gameObject.SetActive(false);
        if (RunIntro)
        {
            GetComponent<AudioSource>().Play();
        }

        firstAchievement  = Resources.Load<AudioClip>("NICEWORK1-Remix");
        almostThere = Resources.Load<AudioClip>("YourAlmostThere1-Remix");

    }

    // Update is called once per frame
    void Update () {
         // Check if no wormhole abort
        if (Transporter == null)
        {
            Debug.Log("No transporter. Please set!");
            return;
        }

        // Get the position
        Vector3 transporter = Transporter.transform.position;
        Vector3 camera = Camera.main.transform.position;
        Vector3 yDifference = transporter - camera;

        TransporterBehaviour behaviour = Transporter.GetComponent<TransporterBehaviour>();
        AudioSource audioSource = GetComponent<AudioSource>();

        //Debug.Log("Camera " + camera.y + " Transporter " + transporter.y + " Difference " + yDifference.y);

        // Test for movement
        if (behaviour.TransporterActive)
        {
            if (yDifference.y > .5)
            {
                // Get Transporter behaviour script and call methods on it
                behaviour.TriggerDown();
            }
            else
            {
                behaviour.TriggerUp();
            }

            // Test getting an achievement
            if (behaviour.AchievementCount == 1 && !PlayedFirstAchievement)
            {
                Debug.Log("About to play achievement 1");
                audioSource.clip = firstAchievement;
                audioSource.Play();
                PlayedFirstAchievement = true;
            }

            // Test for 8 seconds remaining
            if (behaviour.TimeLeft < 8 && !Played8SecRemaining)
            {
                Debug.Log("About to 8 sec warning");
                audioSource.clip = almostThere;
                audioSource.Play();
                Played8SecRemaining = true;
            }
        }



    }

}

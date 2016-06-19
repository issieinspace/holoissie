using UnityEngine;
using System.Collections;

public class TransporterBehaviour : MonoBehaviour
{

    public GameObject AlienPrefab;

    AudioClip explosion = null;
    AudioClip achievement = null;
    AudioClip timeDone = null;
    AudioClip bleep = null;

    AudioSource audioSource = null;

    Color downColor = Color.red;
    Color upColor = Color.red;
    Color doneColor = Color.green;
    Color originalColor;

    string triggeredDirection = "";

    public float TimeLeft = 500;
    int MoveCount = 0;
    public int AchievementLevel = 5;
   
    public int AchievementCount = 0;

    public bool AchievementAttained = false;
    public bool TransporterComplete = false;
    public bool TransporterActive = false;
    public Transform Camera;

    public System.Collections.Generic.List<GameObject> Aliens;

    // Use this for initialization
    void Start()
    {
        Aliens = new System.Collections.Generic.List<GameObject>();

        SpawnAlien();       

        Renderer rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;

        explosion = Resources.Load<AudioClip>("Explosion_Small");
        achievement = Resources.Load<AudioClip>("EtherealAccent");
        timeDone = Resources.Load<AudioClip>("SynthZap");
        bleep = Resources.Load<AudioClip>("Computer04");

    }

    // Update is called once per frame
    void Update()
    {
        if(TransporterActive)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft < 0)
            {
                TimeDone();
            }
        }
  
    }

    void SpawnAlien()
    {
        GameObject alien = GameObject.Instantiate(AlienPrefab);
        alien.transform.SetParent(this.transform);
        alien.transform.localPosition = new Vector3(0, -5f, 0);
        alien.transform.Rotate(0, -180, 0);
        alien.GetComponent<AlienBehaviour>().Target = Camera;

        Aliens.Add(alien);
    }

    public void TriggerDown()
    {
        if(!System.String.Equals(triggeredDirection, "down"))
        {
            Debug.Log("Down triggered!" + triggeredDirection);

            audioSource.clip = bleep;
            audioSource.Play();
           

            Renderer rend = GetComponent<Renderer>();
            rend.material.color = downColor;

            triggeredDirection = "down";
        }
    }


    public void TriggerUp()
    {
       
        if (System.String.Equals(triggeredDirection, "down"))
        {
            Debug.Log(triggeredDirection);

            Renderer rend = GetComponent<Renderer>();
            rend.material.color = upColor;

            audioSource.clip = bleep;
            audioSource.Play();

            triggeredDirection = "up";
            MoveCount++;

            if(MoveCount >= AchievementLevel)
            {
                Debug.Log("Achieved!");
                TriggerAchievement();
            }
            Debug.Log("Up triggered!" + triggeredDirection + "MoveCount" + MoveCount + " Achievement achieved" + AchievementAttained + " Level " + AchievementLevel);
        }

    }

    public void TriggerAchievement()
    {
        // Achievement action happens!!
        Debug.Log("you did good");
        audioSource.clip = achievement;
        audioSource.Play();
        AchievementAttained = true;
        
        // Release the current alien
        GameObject alien = getCurrentAlien();
        alien.GetComponent<AlienBehaviour>().OnDrop();

        // Spawn a new alien
        SpawnAlien();

        MoveCount = 0;
        AchievementCount++;
    }

    GameObject getCurrentAlien()
    {
        return Aliens[Aliens.Count-1];
    }

    public void TimeDone()
    {
        if (!TransporterComplete)
        {
            Debug.Log("TIME IS DONE. You got " + AchievementCount + " achievements");
            audioSource.clip = timeDone;
            audioSource.Play();
            TransporterComplete = true;
            TransporterActive = false;

            Renderer rend = GetComponent<Renderer>();
            rend.material.color = doneColor;
        }

        // Display some info about what you did
    }

    public void OnReset()
    {
        TimeLeft = 500;
        MoveCount = 0;
        AchievementCount = 0;
        AchievementAttained = false;
        TransporterComplete = false;
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = originalColor;
    }



}

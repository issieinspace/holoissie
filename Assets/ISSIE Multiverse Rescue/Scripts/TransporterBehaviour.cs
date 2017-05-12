using UnityEngine;
using System.Collections;
using Timers;
using System;
using Prime31.MessageKit;


public class TransporterBehaviour : MonoBehaviour, ITriggerable
{

    public GameObject AlienPrefab;
    
    AudioClip startTransporter = null;
    AudioClip timeDone = null;
    AudioClip bleep = null;

    AudioSource audioSource = null;

    Color downColor = Color.red;
    Color upColor = Color.red;
    Color doneColor = Color.green;
    
    public Transform Camera;
  
    public DoorBehavior Door;
    public MultiverseRescueCountDownDisplay CountDown;
    public ExerciseTimer ExerciseTimer;
    public String MyExercise;
    public System.Collections.Generic.List<GameObject> Aliens;

    public void Activate()
    {
        Debug.Log("TRANSPORTER ACTIVATED " + this.name);
        MessageKit<string>.addObserver(MessageType.OnReady, (name) => OnReady(name));
        MessageKit.addObserver(MessageType.OnStart, OnStart);
        MessageKit.addObserver(MessageType.OnDisplacementAchieved, OnDisplacementAchieved);
        MessageKit.addObserver(MessageType.OnMoveComplete, OnMoveComplete);
        MessageKit.addObserver(MessageType.OnAchievement, OnAchievement);
        MessageKit.addObserver(MessageType.OnTimeDone, OnTimeDone);
    }

    public void Deactivate()
    {
        MessageKit<string>.removeObserver(MessageType.OnReady, (name) => OnReady(name));
        MessageKit.removeObserver(MessageType.OnStart, OnStart);
        MessageKit.removeObserver(MessageType.OnDisplacementAchieved, OnDisplacementAchieved);
        MessageKit.removeObserver(MessageType.OnMoveComplete, OnMoveComplete);
        MessageKit.removeObserver(MessageType.OnAchievement, OnAchievement);
        MessageKit.removeObserver(MessageType.OnTimeDone, OnTimeDone);

    }

    // Use this for initialization
    void Start()
    {
        Aliens = new System.Collections.Generic.List<GameObject>();

        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;

        startTransporter = Resources.Load<AudioClip>("EtherealAccent");
        timeDone = Resources.Load<AudioClip>("SynthZap");
        bleep = Resources.Load<AudioClip>("Computer04");

        Door = GameObject.Find("Door").GetComponent<DoorBehavior>();
        CountDown = GameObject.Find("Countdown").GetComponent<MultiverseRescueCountDownDisplay>();

      
    }

    public void OnReady(string exerciseName)
    {
        Debug.Log("Hey transporter on ready is being called");
        Door.TriggerReady();
        TimersManager.SetTimer(this, .5f, FlyIn);
    }

    void SpawnAlien()
    {
        GameObject alien = GameObject.Instantiate(AlienPrefab);
        alien.transform.SetParent(this.transform);
        alien.transform.localPosition = new Vector3(0, 1.5f, 0);
        alien.transform.Rotate(0, -180, 0);
        alien.GetComponent<AlienBehaviour>().Target = Camera;
        alien.AddComponent<AlienScaleAnimation>();

        Aliens.Add(alien);
    }

   
    public void OnDisplacementAchieved()
    {
        Debug.Log("Down triggered on Transporter!");

        audioSource.clip = bleep;
        audioSource.Play();

        // Let's see if we can live without the transporter color
        //Renderer rend = GetComponent<Renderer>();
        //rend.material.color = downColor;
     }


    public void OnMoveComplete()
    {
        Debug.Log("Up triggered on Transporter!");

        //Renderer rend = this.GetComponentInChildren().GetComponent<Renderer>();
        //rend.material.color = upColor;

        audioSource.clip = bleep;
        audioSource.Play();
    }

    public void OnAchievement()
    {
        // Release the current alien
        GameObject alien = getCurrentAlien();
        alien.GetComponent<AlienBehaviour>().OnDrop();

        // Spawn a new alien
        SpawnAlien();
    }

    GameObject getCurrentAlien()
    {
        return Aliens[Aliens.Count-1];
    }

    // Called when exercise starts
    public void OnStart()
    {
            audioSource.clip = startTransporter;
            audioSource.Play();

            SpawnAlien();
    }

    void FlyIn()
    {
        Debug.Log("FLYING IN");

        this.gameObject.AddComponent<TransporterFlyInAnimation>();

        
    }

    public void OnTimeDone()
    {
       // Renderer rend = GetComponent<Renderer>();
       // rend.material.color = doneColor;

        audioSource.clip = timeDone;
        audioSource.Play();

        getCurrentAlien().GetComponent<AlienBehaviour>().OnDrop();

        GameObject parent = transform.parent.gameObject;
        this.gameObject.AddComponent<TransporterFlyOutAnimation>();
    }
    
}




    // To play one clip after another, use coroutines
    //http://docs.unity3d.com/540/Documentation/Manual/Coroutines.html


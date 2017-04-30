using UnityEngine;
using System.Collections;
using Timers;
using System;
using Prime31.MessageKit;

public class TransporterBehaviour : MonoBehaviour
{

    public GameObject AlienPrefab;
    
    AudioClip startTransporter = null;
    AudioClip timeDone = null;
    AudioClip bleep = null;

    AudioSource audioSource = null;

    Color downColor = Color.red;
    Color upColor = Color.red;
    Color doneColor = Color.green;

    
    public bool TransporterActive = false;
    public bool FlyInOnStart = false;
    public bool FlyOutOnComplete = false;

    public Transform Camera;
  
    public DoorBehavior Door;
    public MultiverseRescueCountDownDisplay CountDown;
    public ExerciseTimer ExerciseTimer;
    public String MyExercise;


    public System.Collections.Generic.List<GameObject> Aliens;

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

        MessageKit<string>.addObserver(MessageType.OnReady, (name) => OnReady(name));
        MessageKit.addObserver(MessageType.OnDisplacementAchieved, OnDisplacementAchieved);
        MessageKit.addObserver(MessageType.OnMoveComplete, OnMoveComplete);
        MessageKit.addObserver(MessageType.OnAchievement, OnAchievement);
        MessageKit.addObserver(MessageType.OnStart, OnStart);
        MessageKit.addObserver(MessageType.OnTimeDone, OnTimeDone);
        MessageKit<uint>.addObserver(MessageType.OnTick, (countDown) => OnTick(countDown));
    }

    public void OnReady(string exerciseName)
    {
        //setTransporterActiveByExercise((string)args["exerciseName"]);

        //if ((string)args["exerciseName"] == MyExercise)
        if (exerciseName == MyExercise)
        {
            if (FlyInOnStart)
            {
                Door.TriggerReady();
                TimersManager.SetTimer(this, .5f, FlyIn);
            }
        }

    }

    void SpawnAlien()
    {
        GameObject alien = GameObject.Instantiate(AlienPrefab);
        alien.transform.SetParent(this.transform);
        alien.transform.localPosition = new Vector3(0, -7.5f, 0);
        alien.transform.Rotate(0, -180, 0);
        alien.GetComponent<AlienBehaviour>().Target = Camera;
        alien.AddComponent<AlienScaleAnimation>();

        Aliens.Add(alien);
    }

   
    public void OnDisplacementAchieved()
    {
        if(TransporterActive)
        {
            Debug.Log("Down triggered on Transporter!");

            audioSource.clip = bleep;
            audioSource.Play();

            Renderer rend = GetComponent<Renderer>();
            rend.material.color = downColor;
        }
        
     }


    public void OnMoveComplete()
    {
        if (TransporterActive)
        {
            Debug.Log("Up triggered on Transporter!");

            Renderer rend = GetComponent<Renderer>();
            rend.material.color = upColor;

            audioSource.clip = bleep;
            audioSource.Play();
        }
    }

    public void OnAchievement()
    {
        if (TransporterActive)
        {
            // Release the current alien
            GameObject alien = getCurrentAlien();
            alien.GetComponent<AlienBehaviour>().OnDrop();

            // Spawn a new alien
            SpawnAlien();
        }
    }

    GameObject getCurrentAlien()
    {
        return Aliens[Aliens.Count-1];
    }

    // Called when exercise starts
    public void OnStart()
    {
        if (TransporterActive)
        {
            audioSource.clip = startTransporter;
            audioSource.Play();

            SpawnAlien();
        }        
    }

    void FlyIn()
    {
        Debug.Log("FLYING IN");

        GameObject parent = transform.parent.gameObject;
        parent.AddComponent<TransporterFlyInAnimation>();

        Debug.Log("FLEW IN " + parent.name);
    }

    public void OnTimeDone()
    {
        if (TransporterActive)
        {
            Debug.Log("Transporter doing the timeDone thing");

            Renderer rend = GetComponent<Renderer>();
            rend.material.color = doneColor;

            audioSource.clip = timeDone;
            audioSource.Play();

            getCurrentAlien().GetComponent<AlienBehaviour>().OnDrop();

            if (FlyOutOnComplete)
            {
                GameObject parent = transform.parent.gameObject;
                parent.AddComponent<TransporterFlyOutAnimation>();
            }
        }
    }

    internal void OnTick(uint countDown)
    {
        if (TransporterActive)
        {
            //CountDown.TriggerTick((uint)args["countDown"]);
            CountDown.TriggerTick(countDown);
        }
    }

    internal void setTransporterActiveByExercise(String exerciseName)
    {
        if (exerciseName == MyExercise)
        {
            TransporterActive = true;
        }
        else
        {
            TransporterActive = false;
        }
    }
}




    // To play one clip after another, use coroutines
    //http://docs.unity3d.com/540/Documentation/Manual/Coroutines.html


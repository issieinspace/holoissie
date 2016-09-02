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
    
    public bool TransporterComplete = false;
    public bool TransporterActive = false;
    public Transform Camera;

    public System.Collections.Generic.List<GameObject> Aliens;

    // Use this for initialization
    void Start()
    {
        Aliens = new System.Collections.Generic.List<GameObject>();

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
    
    }

    void SpawnAlien()
    {
        GameObject alien = GameObject.Instantiate(AlienPrefab);
        alien.transform.SetParent(this.transform);
        alien.transform.localPosition = new Vector3(0, -5f, 0);
        alien.transform.Rotate(0, -180, 0);
        alien.GetComponent<AlienBehaviour>().Target = Camera;
        alien.AddComponent<AlienScaleAnimation>();

        Aliens.Add(alien);
    }

    public void TriggerDown()
    {
            Debug.Log("Down triggered on Transporter!");

            audioSource.clip = bleep;
            audioSource.Play();
           
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = downColor;
     }


    public void TriggerUp()
    {

        Debug.Log("Up triggered on Transporter!");
        //if (System.String.Equals(triggeredDirection, "down"))

        Renderer rend = GetComponent<Renderer>();
            rend.material.color = upColor;

            audioSource.clip = bleep;
            audioSource.Play();
    }

    public void TriggerAchievement()
    {
        // Achievement action happens!!
        Debug.Log("Transporter now reacting to achievement");
        audioSource.clip = achievement;
        audioSource.Play();
        
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

    public void OnStart()
    {
        SpawnAlien();
    }

    public void OnReset()
    {
        TransporterComplete = false;
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = originalColor;
        foreach (GameObject alien in Aliens)
        {
            GameObject.DestroyImmediate(alien);
        }
    }

    public void TriggerTimeDone()
    {
        Debug.Log("Transporter doing the timeDone thing");
        TransporterComplete = true;
        TransporterActive = false;

        Renderer rend = GetComponent<Renderer>();
        rend.material.color = doneColor;

        audioSource.clip = timeDone;
        audioSource.Play();
        // Display some info about what you did and where to go next
    }


}




    // To play one clip after another, use coroutines
    //http://docs.unity3d.com/540/Documentation/Manual/Coroutines.html


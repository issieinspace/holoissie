using UnityEngine;
using System.Collections;

public class AlienBehaviour : MonoBehaviour {

    public Transform Target;
    AudioSource audioSource = null;
    AudioClip landingSound = null;
    AudioClip chatterSound = null;
    public bool isdropped = false;
    public bool partytime = false;
    public float Endwalk = 2.0f;
    
    bool alienwalking = false;
    bool alienjumping = false;
    bool alienrunning = false;

    private Animation animator;

    private AnimationClip currentAnimation;
    private Vector3 currentLocation;
    float speed = 1;
    public float countup;
    

    void Start ()
    {
        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;

        landingSound = Resources.Load<AudioClip>("Explosion_Small");
        chatterSound = Resources.Load<AudioClip>("Chatter");
        animator = this.GetComponent<Animation>();
        currentAnimation = animator.GetClip("alien_idle");
    }
	
	// Update is called once per frame
	void Update ()
    {
	    transform.LookAt(Target);
        var rigidbody = this.GetComponent<Rigidbody>();
        if(transform.position.y <= 0.0f && rigidbody !=null)
        {
            DestroyImmediate(rigidbody);
            
        }

        if(animator.isPlaying == false)
        {
            animator.Play(currentAnimation.name);
        }

        countup += Time.deltaTime;

        if (alienwalking)
        {
            transform.Translate(Vector3.left * Endwalk, Space.Self);
        }

        if(alienjumping)
        {
            transform.Translate(Vector3.right * Time.deltaTime, Space.Self);
        }

        if(alienrunning)
        {
           transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        }

        if (isdropped && !partytime)
        {
            Endwalk -= Endwalk;

            if (Endwalk == 0f)
            {
                alienwalking = false;
                currentAnimation = animator.GetClip("alien_idle");
            }
        }


    }


    public bool getisDropped()
    {
        return isdropped;
    }


    public void OnDrop()
    {
        var rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        isdropped = true;
        audioSource.clip = chatterSound;
        audioSource.Play();
        currentAnimation = animator.GetClip("alien_walking");
        //transform.Rotate(Vector3.right * Time.deltaTime * speed);
        alienwalking = true;
        countup += Time.deltaTime;

        transform.parent = null;
    }

    
   
    public void run()
    {
        currentAnimation = animator.GetClip("alien_running");
    }


    public void DanceParty()
    {
        audioSource.clip = chatterSound;
        audioSource.Play();
        partytime = true;
        currentAnimation = animator.GetClip("alien_jumping");

    }

    /*a nice idea, but found that he bounced like crazy
     * void OnTriggerEnter(Collider other)
    {
        Debug.Log("alien collision detected");

        if (other.gameObject.layer == SpatialMapping.PhysicsRaycastMask)
        {
            audioSource.clip = landingSound;
            audioSource.Play();
            audioSource.clip = chatterSound;
            audioSource.Play();
            Debug.Log("Finished handling collision with spatial map");
        } 
    }
    */

    
}





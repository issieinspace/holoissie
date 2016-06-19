using UnityEngine;
using System.Collections;

public class AlienBehaviour : MonoBehaviour {

    public Transform Target;
    AudioSource audioSource = null;
    AudioClip landingSound = null;
    AudioClip chatterSound = null;

    private Animation animator;

    private AnimationClip currentAnimation;

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
   }

    public void OnDrop()
    {
        var rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        audioSource.clip = chatterSound;
        audioSource.Play();
        currentAnimation = animator.GetClip("alien_walking");
        transform.Rotate(Vector3.right * Time.deltaTime);
        
    }


    public void run()
    {
        currentAnimation = animator.GetClip("alien_running");
        this.transform.position = Vector3.left;
    }


    public void DanceParty()
    {
        

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





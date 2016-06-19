using UnityEngine;
using System.Collections;

public class AlienBehaviour : MonoBehaviour {


  

    Vector3 originalPosition;
    Vector3 newposition;
    public GameObject AlienPrefab;
    public Transform Target;

    private Animation animator;

    private AnimationClip currentAnimation;

    void Start ()
    {
        animator = this.GetComponent<Animation>();
        currentAnimation = animator.GetClip("alien_idle");
    }
	
	// Update is called once per frame
	void Update ()
    {
	    transform.LookAt(Target);
        if(animator.isPlaying == false)
        {
            animator.Play(currentAnimation.name);
        }
    }

    public void OnDrop()
    {
        var rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        currentAnimation = animator.GetClip("alien_walking");
    }


    public void run()
    {
        currentAnimation = animator.GetClip("alien_running");
        this.transform.position = Vector3.left;
    }
  



}





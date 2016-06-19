using UnityEngine;
using System.Collections;

public class AlienBehaviour : MonoBehaviour {


  

    Vector3 originalPosition;
    Vector3 newposition;
    public GameObject AlienPrefab;
    public Transform Target;


    void Start ()
    {
    
    }
	
	// Update is called once per frame
	void Update ()
    {
	    transform.LookAt(Target);
    }

    public void OnDrop()
    {
        var rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

  



}





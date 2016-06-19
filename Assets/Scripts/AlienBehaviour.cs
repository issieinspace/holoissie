using UnityEngine;
using System.Collections;

public class AlienBehaviour : MonoBehaviour {


  

    Vector3 originalPosition;
    Vector3 newposition;
    public GameObject AlienPrefab;


    void Start ()
    {
    
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Ondrop()
    {
        var rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void SpawnAliens()
    {
        Instantiate(AlienPrefab, originalPosition, transform.rotation);

    }



}





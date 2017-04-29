using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGrowAnimation : MonoBehaviour {

    public Vector3 StartSize = new Vector3(0,0,0);
    public Vector3 FinalSize = new Vector3(0.5f,0.5f,0.5f);
    public float GrothSpeed = 0.05f;

    private bool isGrowing = false;
	// Use this for initialization
	void Start ()
	{
	    this.transform.localScale = StartSize;
	    this.isGrowing = true;
	}
	

	// Update is called once per frame
	void Update () {
	    if (isGrowing)
	    {
	        Vector3 newScale = this.transform.localScale;
	        newScale.x += GrothSpeed;
	        newScale.y += GrothSpeed;
	        newScale.z += GrothSpeed;
	        transform.localScale = newScale;
	        if (newScale.magnitude >= FinalSize.magnitude)
	            isGrowing = false;
	    }
	}
}

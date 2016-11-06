using UnityEngine;
using System.Collections;
using System;

public class AlienScaleAnimation : MonoBehaviour {

    private Vector3 targetScale;
    private Vector3 startingScale;
    private float totalFrames = 0;
    private float targetFrames = 60;

    // Use this for initialization
    void Start () {
        //Debug.Log("Starting scale up for " + transform.gameObject.GetInstanceID().ToString());
        targetScale = transform.localScale;
        transform.localScale = targetScale * .01f;
        startingScale = targetScale * .01f;
    }
	
	// Update is called once per frame
	void Update () {
        float ratio = totalFrames / targetFrames;
        transform.localScale = Vector3.Lerp(startingScale, targetScale, ratio);
        if(ratio >= 1)
        {
            Complete();
        }
        totalFrames++;
	}

    internal void Complete()
    {
        //Debug.Log("Completing scale up for " + transform.gameObject.GetInstanceID().ToString());

        transform.localScale = targetScale;
        Destroy(this);
    }
}

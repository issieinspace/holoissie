using UnityEngine;
using System.Collections;

public class AlienScaleAnimation : MonoBehaviour {

    private Vector3 targetScale;
    private Vector3 startingScale;
    private float totalFrames = 0;
    private float targetFrames = 60;

    // Use this for initialization
    void Start () {
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
            Destroy(this);
        }
        totalFrames++;
	}
}

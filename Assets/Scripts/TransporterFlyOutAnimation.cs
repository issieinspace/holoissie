using UnityEngine;
using System.Collections;

public class TransporterFlyOutAnimation : MonoBehaviour {

    private Vector3 target;
    private Vector3 starting;
    private float totalFrames = 0;
    private float targetFrames = 30;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Started the thing");
        starting = transform.position;
        target = transform.position + transform.up * -3;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Doing update the thing");
        float ratio = totalFrames / targetFrames;
        
        transform.position = Vector3.Lerp(starting, target, ratio);
        if (ratio >= 1)
        {
            
            Destroy(this);
        }
        totalFrames++;
    }
}

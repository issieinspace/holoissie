using UnityEngine;
using System.Collections;

public class CreditsAnimation : MonoBehaviour {

    private Vector3 target;
    private Vector3 starting;
    private float totalFrames = 0;
    private float targetFrames = 900;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Started Credits Roll");
        starting = transform.localPosition;
        target = new Vector3(-1f, 7f, 0.5f);//transform.position + transform.forward * -2;

    }

    // Update is called once per frame
    void Update()
    {
        float ratio = totalFrames / targetFrames;

        transform.localPosition = Vector3.Lerp(starting, target, ratio);
        if (ratio >= 1)
        {
            transform.gameObject.SetActive(false);
            Destroy(this);
            
        }
        totalFrames++;
    }
}

using UnityEngine;
using System.Collections;

public class FlyInAnimation : MonoBehaviour {

    public float _targetY;
    private Vector3 _target;
    private Vector3 _starting;
    private float _totalFrames = 0;
    public float _targetFrames = 30;
    private bool _started = false;

    // Use this for initialization
    public void Setup(Vector3 targetPosition)
    {
        Debug.Log("Setup Flying In");
        _starting = transform.localPosition;
        _target = targetPosition;
        _started = true;
        Debug.Log("Starting Flying in");
    }

    // Update is called once per frame
    void Update()
    {
        if(_started)
        {
            float ratio = _totalFrames / _targetFrames;

            transform.localPosition = Vector3.Lerp(_starting, _target, ratio);
            if (ratio >= 1)
            {
                Destroy(this);
            }
           _totalFrames++;
        }
   
    }
}

using UnityEngine;
using System.Collections;
using System;
using Prime31.MessageKit;

public class WaterGrower : MonoBehaviour, IGrowable
{
    public float expectedHeightChange = 1f;
    public float maxHeightChange = 1.35f;
    public int numOfSteps = 5;
    public float speed = 0.15f;

    private float startingHeight;
    private float heightChange;
    private float nextHeight;


    public void Activate()
    {
        MessageKit.addObserver(MessageType.OnMoveComplete, Raise);
        MessageKit.addObserver(MessageType.OnSpacialMappingComplete, Setup);
    }

    public void Deactivate()
    {
        MessageKit.removeObserver(MessageType.OnMoveComplete, Raise);
    }

    void Start()
    {
        heightChange = expectedHeightChange / numOfSteps;
    }

    void Setup()
    {
        startingHeight = transform.position.y;
    }

    void Raise()
    {
        if (transform.position.y < startingHeight + maxHeightChange)
        {
            StartCoroutine(RaiseOverTime());
        }
        
    }

    IEnumerator RaiseOverTime()
    {

        nextHeight = Mathf.Clamp(transform.position.y + heightChange,
                                 float.MinValue,
                                 startingHeight + maxHeightChange);
        while (true)
        {
            Vector3 newPosition = transform.position;
            float frameGrowth = speed * Time.deltaTime;
            newPosition.y += frameGrowth;
            transform.position = newPosition;
            if (newPosition.y >= nextHeight)
            {
                newPosition.y = nextHeight;
                transform.position = newPosition;
                break;
            }
            yield return null;
        }
    }

}

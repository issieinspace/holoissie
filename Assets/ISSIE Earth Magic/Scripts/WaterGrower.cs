using UnityEngine;
using System.Collections;
using System;
using Prime31.MessageKit;

public class WaterGrower : MonoBehaviour, IGrowable
{
    public float expectedHeightChange = 1f;
    public int numOfSteps = 5;
    public float speed = 0.15f;

    private float startingHeight;
    private float heightChange;
    private float nextHeight;


    public void Activate()
    {
        MessageKit.addObserver(MessageType.OnMoveComplete, Raise);
    }

    public void Deactivate()
    {
        MessageKit.removeObserver(MessageType.OnMoveComplete, Raise);
    }

    void Start()
    {
        heightChange = expectedHeightChange / numOfSteps;
    }

    void Raise()
    {
        StartCoroutine(RaiseOverTime());
    }

    IEnumerator RaiseOverTime()
    {
        nextHeight = transform.position.y + heightChange;
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

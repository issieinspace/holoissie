using UnityEngine;
using System.Collections;
using System;
using Prime31.MessageKit;

public class WaterGrower : MonoBehaviour, ITriggerable
{
    public float expectedHeightChange = 1f;
    public float maxHeightChange = 1.35f;
    public int numOfSteps = 5;
    public float speed = 0.15f;

    private float startingHeight;
    private float heightChange;
    private float nextHeight;
    public float volume;
    private AudioSource audioSource;


    public void Activate()
    {
        MessageKit.addObserver(MessageType.OnMoveComplete, Raise);

        audioSource.volume = volume;
        audioSource.Play();
    }

    public void Deactivate()
    {
        MessageKit.removeObserver(MessageType.OnMoveComplete, Raise);
        StartCoroutine(AudioOff());
    }

    void Start()
    {
        heightChange = expectedHeightChange / numOfSteps;
        audioSource = GetComponent<AudioSource>();
        MessageKit.addObserver(MessageType.OnSpatialMappingComplete, Setup);
    }

    void Setup()
    {
        startingHeight = transform.position.y;
        MessageKit.removeObserver(MessageType.OnSpatialMappingComplete, Setup);
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

        nextHeight = transform.position.y + heightChange;
        if (nextHeight > startingHeight + maxHeightChange)
        {
            nextHeight = startingHeight + maxHeightChange;
        }

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

    IEnumerator AudioOff()
    {
        float speed = 0.4f;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= speed * Time.deltaTime;
            if (audioSource.volume < 0)
            {
                audioSource.volume = 0;
                audioSource.Stop();
            }
            yield return null;
        }
    }

}

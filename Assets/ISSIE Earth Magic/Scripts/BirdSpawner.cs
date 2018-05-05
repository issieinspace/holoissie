using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Prime31.MessageKit;

public class BirdSpawner : MonoBehaviour, ITriggerable
{
    public GameObject Prop;

    public void Activate()
    {
        MessageKit.addObserver(MessageType.OnMoveComplete, CreateProp);
        MessageKit.addObserver(MessageType.OnAchievement, CreateManyProps);
    }

    public void Deactivate()
    {
        MessageKit.removeObserver(MessageType.OnMoveComplete, CreateProp);
        MessageKit.removeObserver(MessageType.OnAchievement, CreateManyProps);
    }

    void CreateProp()
    {
        GameObject bird = GameObject.Instantiate(Prop);
        bird.transform.position = new Vector3(0,0,0);
    }

    void CreateManyProps()
    {
        for(int i = 0; i< 5; i++)
            CreateProp();
    }

    void Start()
    {
        
    }

    void Update()
    {
    }




}
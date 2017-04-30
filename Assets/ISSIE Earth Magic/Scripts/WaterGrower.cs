using UnityEngine;
using System.Collections;
using System;
using Prime31.MessageKit;

public class WaterGrow : MonoBehaviour, IGrowable
{
    public float heightToRaise = 1f;


    public void Activate()
    {
        
    }

    public void Deactivate()
    {
        throw new NotImplementedException();
    }

    void Start()
    {

    }

}

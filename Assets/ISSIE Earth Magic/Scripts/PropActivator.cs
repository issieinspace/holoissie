using Prime31.MessageKit;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class PropActivator : MonoBehaviour
{
    public string exerciseName;
    private IGrowable growable;

    void Start()
    {
        growable = GetComponent<IGrowable>();

        MessageKit<string>.addObserver(MessageType.OnReady, (name) =>
        {
            if (name == exerciseName)
            {
                growable.Activate();
            }
            else
            {
                growable.Deactivate();
            }
        });
    }


}

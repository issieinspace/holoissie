using Prime31.MessageKit;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class PropActivator : MonoBehaviour
{
    public string exerciseName;
    private ITriggerable triggerable;

    void Start()
    {
        triggerable = GetComponent<ITriggerable>();

        MessageKit<string>.addObserver(MessageType.OnReady, (name) =>
        {
            if (name == exerciseName)
            {
                triggerable.Activate();
            }
            else
            {
                triggerable.Deactivate();
            }
        });
    }


}

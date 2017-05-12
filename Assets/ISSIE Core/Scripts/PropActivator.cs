using Prime31.MessageKit;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class PropActivator : MonoBehaviour
{
    public string exerciseName;
    private ITriggerable triggerable;
    private bool isActive = false;

    void Start()
    {
        triggerable = GetComponent<ITriggerable>();

        MessageKit<string>.addObserver(MessageType.OnReady, (name) =>
        {
            if (name == exerciseName && !isActive)
            {
                isActive = true;
                triggerable.Activate();
            }
            else if (isActive)
            {
                triggerable.Deactivate();
            }
        });
    }


}

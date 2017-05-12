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
        if(exerciseName.Length < 1)
        {
            exerciseName = gameObject.name;
            Debug.Log("Exercise name was null so set it to " + exerciseName);
        }

        MessageKit<string>.addObserver(MessageType.OnPlayerReady, (name) =>
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

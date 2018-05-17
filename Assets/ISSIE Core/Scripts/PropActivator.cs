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
    public bool isSetup = false;

    void Start()
    {
        if (!isSetup)
        {
            Setup();
        }
    }

    public void Setup()
    {
        triggerable = GetComponent<ITriggerable>();
        if(exerciseName.Length < 1)
        {
            exerciseName = gameObject.name;
            Debug.Log("Exercise name was null so set it to " + exerciseName);
        }

        MessageKit<string>.addObserver(MessageType.OnPlayerReady, (name) =>
        {
            if (name == exerciseName && !isActive)
            {
                isActive = true;
                triggerable.Activate();
                Debug.Log("Activated " + this.gameObject.name + " for " + exerciseName);
            }
            else if (isActive)
            {
                triggerable.Deactivate();
                Debug.Log("DEactivated " + this.gameObject.name + " for " + exerciseName);
            }
        });

        isSetup = true;
    }


}

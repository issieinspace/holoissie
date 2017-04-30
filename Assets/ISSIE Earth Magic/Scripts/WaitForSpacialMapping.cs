using UnityEngine;
using System.Collections;
using Prime31.MessageKit;

public class WaitForSpacialMapping : MonoBehaviour
{


    void Awake()
    {
        MessageKit.addObserver(MessageType.OnSpacialMappingComplete, Activate);
        gameObject.SetActive(false);
    }

    void Activate()
    {
        gameObject.SetActive(true);
        MessageKit.removeObserver(MessageType.OnSpacialMappingComplete, Activate);
    }
}

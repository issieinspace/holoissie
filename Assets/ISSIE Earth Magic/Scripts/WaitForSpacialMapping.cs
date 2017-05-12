using UnityEngine;
using System.Collections;
using Prime31.MessageKit;

public class WaitForSpacialMapping : MonoBehaviour
{


    void Start()
    {
        gameObject.SetActive(false);
        MessageKit.addObserver(MessageType.OnSpacialMappingComplete, Activate);
    }

    void Activate()
    {
        gameObject.SetActive(true);
        MessageKit.removeObserver(MessageType.OnSpacialMappingComplete, Activate);
    }
}

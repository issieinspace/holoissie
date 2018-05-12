using UnityEngine;
using System.Collections;
using Prime31.MessageKit;

public class WaitForSpacialMapping : MonoBehaviour
{


    void Start()
    {
        gameObject.SetActive(false);
        MessageKit.addObserver(MessageType.OnSpatialMappingComplete, Activate);
    }

    void Activate()
    {
        gameObject.SetActive(true);
        MessageKit.removeObserver(MessageType.OnSpatialMappingComplete, Activate);
    }
}

using UnityEngine;
using System.Collections;

public class Monitor : MonoBehaviour
{

    public bool Debug;

    // Use this for initialization
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        if (Debug)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayMessage(string message)
    {
        GetComponent<TextMesh>().text = message;
  
    }
}


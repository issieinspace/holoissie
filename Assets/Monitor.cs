using UnityEngine;
using System.Collections;

public class Monitor : MonoBehaviour
{

    public bool Debug;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {

        if (Debug)
        {
            // Make a background box
            GUI.Box(new Rect(10, 10, 400, 400), "Actions");


        }
    }
}


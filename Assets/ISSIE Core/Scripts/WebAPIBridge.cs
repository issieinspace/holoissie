using BestHTTP;
using System;
using UnityEngine;

public class WebAPIBridge : MonoBehaviour
{
    public CycleReadout cycleReadout;
    public string TargetURL;

    private void GetCycleReadout()
    {
        
        HTTPRequest request = new HTTPRequest(new Uri(TargetURL), OnRequestFinished);
        request.Send();
        
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        string data = response.DataAsText;
        cycleReadout = JsonUtility.FromJson<CycleReadout>(data);
        WriteDiagnostics("Elapsed distance: " + cycleReadout.distance);
        Debug.Log("Requested " + TargetURL + " Text received: " + response.DataAsText);
    }

    private void WriteDiagnostics(string data)
    {
       GameObject.Find("Diagnostic").GetComponent<Monitor>().DisplayMessage(data);
    }

    void Update()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            GetCycleReadout();
        }
    }
}

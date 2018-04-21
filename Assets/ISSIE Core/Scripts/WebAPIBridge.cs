﻿using BestHTTP;
using HoloToolkit.Unity;
using System;
using UnityEngine;

public class WebAPIBridge : Singleton<WebAPIBridge>
{
    public CycleReadout cycleReadout;
    public string TargetURL;

    public void GetCycleReadout()
    {
        WriteDiagnostics("Requesting " + TargetURL);
        HTTPRequest request = new HTTPRequest(new Uri(TargetURL), OnRequestFinished);
        request.Send();
        
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        string data = response.DataAsText;
        cycleReadout = JsonUtility.FromJson<CycleReadout>(data);
        //WriteDiagnostics("Elapsed distance: " + cycleReadout.distance);
        WriteDiagnostics("Requested " + TargetURL + "\n Text received: " + response.DataAsText);
    }

    private void WriteDiagnostics(string data)
    {
       GameObject.Find("Log").GetComponent<Monitor>().DisplayMessage(data);
    }

    void Update()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            GetCycleReadout();
        }
    }
}

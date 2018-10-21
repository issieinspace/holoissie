using BestHTTP;
using HoloToolkit.Unity;
using System;
using UnityEngine;

public class WebAPIBridge : Singleton<WebAPIBridge>
{
    public CycleReadout cycleReadout;
    public string TargetURL;
    public string TargetResetURL;

    public void GetCycleReadout()
    {
        WriteDiagnostics("Requesting " + TargetURL);
        HTTPRequest request = new HTTPRequest(new Uri(TargetURL), OnRequestFinished);
        request.Send();
        
    }

    public void ResetReadout()
    {
        HTTPRequest request = new HTTPRequest(new Uri(TargetResetURL));
        request.Send();
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        string data = response.DataAsText;
        //data = System.Text.Encoding.UTF8.GetString(response.Data, 3, response.Data.Length - 3);
        WriteDiagnostics("Requested " + TargetURL + "\n Text received: " + response.DataAsText);
        cycleReadout = JsonUtility.FromJson<CycleReadout>(data);
        //WriteDiagnostics("Elapsed distance: " + cycleReadout.distance);
        WriteDiagnostics("Requested " + TargetURL + "\n Text received: " + response.DataAsText);
    }

    private void WriteDiagnostics(string data)
    {
        Debug.Log(data);
       //GameObject.Find("Log").GetComponent<Monitor>().DisplayMessage(data);
    }

    void Update()
    {
        //if (Input.GetAxis("Fire1") > 0)
        //{
        //    GetCycleReadout();
        //}
    }
}

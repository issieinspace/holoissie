using BestHTTP;
using System;
using UnityEngine;

public class StartupDiagnostics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	    HTTPRequest request = new HTTPRequest(new Uri("http://issiepi.local:9000"), OnRequestFinished);
        request.Send();
        
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        string data = response.DataAsText;
        this.GetComponent<Monitor>().DisplayMessage("online");
        this.GetComponent<Monitor>().DisplayMessage("");
    }
}

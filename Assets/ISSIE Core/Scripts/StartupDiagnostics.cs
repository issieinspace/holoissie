using BestHTTP;
using System;
using UnityEngine;

public class StartupDiagnostics : MonoBehaviour {

    public string _message;

	// Use this for initialization
	void Start () {
		
	    HTTPRequest request = new HTTPRequest(new Uri("http://192.168.86.78:9000"), OnRequestFinished);
        request.Send();
        
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        string data = response.DataAsText;
      
        this.GetComponent<TextMesh>().text = _message;
    }
}

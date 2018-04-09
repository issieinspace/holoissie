using System.Collections;

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Networking;

public class WebAPIBridge : Singleton<WebAPIBridge> {

    public CycleReadout cycleReadout;
    public string URL = "http://issiepi.local:9000/rpm_read";

    // Use this for initialization
    void Start () {
        StartCoroutine(GetCycleReadout());
	}

    private IEnumerator GetCycleReadout()
    {
        string TargetURL = URL; //"http://issiepi.local:9000/rpm_read";
        Debug.Log("Going to hit " + TargetURL);
        UnityWebRequest webRequest = UnityWebRequest.Get(TargetURL);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            Debug.Log("Got data: " + data);
            GameObject.Find("Diagnostic").GetComponent<Monitor>().DisplayMessage(data);
            cycleReadout = JsonUtility.FromJson<CycleReadout>(data);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

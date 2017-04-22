using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.IO;

public class MoCap : MonoBehaviour {

    public System.Collections.Generic.List<DataPoint> Sessions;
    public string FilePath;
    public bool IsSessionInProgress;
    public float ElapsedTime;
    public string Tag;
    public Session CurrentSession;
    public DataPoint[] SessionData;
    
    public class DataPoint
    {
        public Vector3 Vector { get; set; }
        public float Time { get; set; }
        public string Tag { get; set; }
    }

    public class Session
    {
        public string title;
        System.Collections.Generic.List<DataPoint> DataPoints;

        public Session()
        {

        }
    }

    // Use this for initialization
    void Start()
    {
        FilePath = "MoCap.csv";
    }
    
    void Update()
    {
        if(IsSessionInProgress)
        {
            CaptureDataPoint();
        }
    }

    private void CaptureDataPoint()
    {
        DataPoint dataPoint = new DataPoint();

    }

    public void StartSession()
    {
        CurrentSession = new Session();
        IsSessionInProgress = true;
        ElapsedTime = 0;
    }

    public void EndSession()
    {
        IsSessionInProgress = false;
        WriteCsv();
    }

    void WriteCsv()
    {
        var csv = new StringBuilder();
        csv.AppendLine("Tag,Time,X,Y,Z");

        //in your loop
        foreach (DataPoint datapoint in SessionData)
        {
            var newLine = datapoint.ToString();
            csv.AppendLine(newLine);
        }
        File.WriteAllText(FilePath, csv.ToString());
        
    }
}

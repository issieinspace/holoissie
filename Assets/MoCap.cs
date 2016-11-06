using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.IO;

public class MoCap : MonoBehaviour {

    System.Collections.Generic.List<DataPoint> SessionData;
    public string FilePath;
    
    class DataPoint
    {
        public Vector3 vector { get; set; }
        public float time;
        public string tag;
    }

	// Use this for initialization
	void Start () {
        

    }
    

    void WriteCsv()
    {
        var csv = new StringBuilder();
        csv.AppendLine("Tag,Time,X,Y,Z");

        //in your loop
        foreach (DataPoint datapoint in SessionData)
        {
            var newLine = string.Format("{0},{1},{2},{3},{4}", datapoint.tag, datapoint.time, datapoint.vector.x, datapoint.vector.y, datapoint.vector.z);
            csv.AppendLine(newLine);
        }
        File.WriteAllText(FilePath, csv.ToString());
        
    }
}

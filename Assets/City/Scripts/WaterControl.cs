using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour {
    public GameObject water;
	// Use this for initialization
	void Start () {
        water.transform.position = new Vector3(water.transform.position.x, 0, water.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        if (water.transform.position.y<5f) {
            water.transform.position = new Vector3(water.transform.position.x, water.transform.position.y + 0.001f, water.transform.position.z);
        }   
    }
}

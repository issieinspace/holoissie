using UnityEngine;
using System.Collections;

public class WormholeBehaviour : MonoBehaviour {

    public GameObject AlienPrefab;

	// Use this for initialization
	void Start () {
	    if(AlienPrefab != null)
        {
            GameObject alien = GameObject.Instantiate(AlienPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}

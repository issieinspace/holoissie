using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Wormhole;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Check if no wormhole abort
        if (Wormhole==null)
        {
            return;
        }

        // Get the position
        float wormholeY = Wormhole.transform.position.y;
        float cameraY = Camera.main.transform.position.y;

        if(wormholeY - cameraY > .5)
        {
           // Get Wormhole behaviour script and call methods on it

            

        }
    }

    // ALIENS can look for gameplay manager
    // 
}

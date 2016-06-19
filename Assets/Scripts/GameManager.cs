using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Transporter;
    public bool RunIntro = true;

	// Use this for initialization
	void Start () {
        // Call this when audio source finishes playing: SpatialMapping.Instance.DrawVisualMeshes = false;
        //SpatialMapping.Instance.gameObject.SetActive(false);
        if(RunIntro)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update () {
         // Check if no wormhole abort
        if (Transporter == null)
        {
            Debug.Log("No transporter. Please set!");
            return;
        }

        // Get the position
        Vector3 transporter = Transporter.transform.position;
        Vector3 camera = Camera.main.transform.position;
        Vector3 yDifference = transporter - camera;

        TransporterBehaviour behaviour = Transporter.GetComponent<TransporterBehaviour>();

        
        //Debug.Log("Camera " + camera.y + " Transporter " + transporter.y + " Difference " + yDifference.y);
        

        if (behaviour.TransporterActive)
        {
            if (yDifference.y > .5)
            {
                // Get Transporter behaviour script and call methods on it
                behaviour.TriggerDown();
            }
            else
            {
                behaviour.TriggerUp();
            }
        }

    }

}

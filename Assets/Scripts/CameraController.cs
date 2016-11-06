using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float speed = 2;
        //float rotationSpeed = 20;
	    float translationY = Input.GetAxis("Vertical") * speed;
        float translationX = Input.GetAxis("Horizontal") * speed;
        float translationZ = Input.GetAxis("Depth") * speed;
        translationY *= Time.deltaTime;
        translationX *= Time.deltaTime;
        translationZ *= Time.deltaTime;


        transform.Translate(translationX, translationY, translationZ);
        //transform.Rotate(0, rotation, 0);
        
	}
}

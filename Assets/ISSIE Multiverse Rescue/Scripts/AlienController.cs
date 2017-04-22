using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float speed = 2;
        float translationX = Input.GetAxis("Horizontal") * speed;
        float translationY = Input.GetAxis("Vertical") * speed;
        float translationZ = Input.GetAxis("Depth") * speed;

        translationX *= Time.deltaTime;
        translationY *= Time.deltaTime;
        translationZ *= Time.deltaTime;

        transform.Translate(translationX, translationY, translationZ);
       
    }
}

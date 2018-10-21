using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureSetter : MonoBehaviour {

    public GameObject NewImage;
    public GameObject OldImage;

	// Use this for initialization
	void Start () {
		
	}
	
    public void SetImage(Sprite sprite)
    {
        OldImage.GetComponent<Animator>().Rebind();
        NewImage.GetComponent<Animator>().Rebind();
        OldImage.GetComponent<Image>().sprite = NewImage.GetComponent<Image>().sprite;
        NewImage.GetComponent<Image>().sprite = sprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ViewingPane
{
    LeftPane,
    RighPane,
    FrontPane,
    None
}

public class BicycleManager : MonoBehaviour {
    public GameObject FrontPane;
    public GameObject LeftPane;
    public GameObject RightPane;
    public GameObject LeftPortal;
    public GameObject RightPortal;

    public float DistancePerPhoto = 800;
    public float intervalMillis = 100;
    public GameObject pictureBucket;
    public PictureListManager pictureListManager;

    private float elapsedMillis = 0;
    private CycleReadout cycleReadOut;

    private float leftPaneProgress = 0;
    private float rightPaneProgress = 0;
    private ViewingPane lastActivePane = ViewingPane.LeftPane;

    private float distanceSinceLastUpdate;


	// Use this for initialization
	void Start () {
        cycleReadOut = new CycleReadout();
        pictureListManager = pictureBucket.GetComponent<PictureListManager>();
	}
	
	// Update is called once per frame
	void Update () {

        // Gaze
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
            if (hitInfo.collider.gameObject.tag == "LeftPictureFrame")
            {
                lastActivePane = ViewingPane.LeftPane;
            }
            else if(hitInfo.collider.gameObject.tag == "RightPictureFrame")
            {
                lastActivePane = ViewingPane.RighPane;
            }

        }

        elapsedMillis += Time.deltaTime * 1000;
        if(elapsedMillis > intervalMillis)
        {
            // update RPM/Distance/Speed
            cycleReadOut.distance += 10;
            cycleReadOut.rpm = 20;
            cycleReadOut.speed = 14;
            distanceSinceLastUpdate = 10;
            elapsedMillis = 0;
        }
        UpdateLeftPane();
        UpdateRightPane();
        UpdateFrontPane();
	}

    void UpdateLeftPane()
    {
        if(lastActivePane == ViewingPane.LeftPane)
        {
            leftPaneProgress += distanceSinceLastUpdate;
            //LeftPane.GetComponentInChildren<Image>().fillAmount = leftPaneProgress/DistancePerPhoto;
            LeftPortal.GetComponentInChildren<Image>().fillAmount = leftPaneProgress/DistancePerPhoto;

            if (leftPaneProgress > DistancePerPhoto)
            {
                leftPaneProgress = 0;
                SetNextPicture(LeftPane, pictureListManager.PictureSprites[(int)(Random.value * pictureListManager.PictureSprites.Count)]);
            }
        }
    }

    void SetNextPicture(GameObject Pane, Sprite sprite)
    {
        Pane.GetComponent<PictureSetter>().SetImage(sprite);
    }

    void UpdateRightPane()
    {
        if(lastActivePane == ViewingPane.RighPane)
        {
            rightPaneProgress += distanceSinceLastUpdate;
            //RightPane.GetComponentInChildren<Image>().fillAmount = rightPaneProgress / DistancePerPhoto;
            RightPortal.GetComponentInChildren<Image>().fillAmount = rightPaneProgress / DistancePerPhoto;

            if (rightPaneProgress > DistancePerPhoto)
            {
                rightPaneProgress = 0;
                SetNextPicture(RightPane, pictureListManager.PictureSprites[(int)(Random.value * pictureListManager.PictureSprites.Count)]);
            }
        }
    }

    void UpdateFrontPane()
    {

    }
}

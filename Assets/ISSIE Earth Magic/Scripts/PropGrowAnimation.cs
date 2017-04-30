using Prime31.MessageKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropGrowAnimation : MonoBehaviour, IGrowable
{

    public Vector3 StartSize = new Vector3(0,0,0);
    public Vector3 FinalSize = new Vector3(0.5f,0.5f,0.5f);
    public float GrowthSpeed = 0.15f;
    public int numberOfSteps = 5;

    private Vector3 growthAmount;
    private Vector3 nextScale;
	
	void Start()
	{
	    this.transform.localScale = StartSize;
        FinalSize = FinalSize * (1 + Random.Range(-0.05f, 0.05f));
        growthAmount = (FinalSize - StartSize) / numberOfSteps;
        MessageKit.addObserver(MessageType.OnMoveComplete, Grow);
	}
	

	// Update is called once per frame
	/*void Update () {
	    if (isGrowing)
	    {
	        Vector3 newScale = this.transform.localScale;
	        newScale.x += GrothSpeed;
	        newScale.y += GrothSpeed;
	        newScale.z += GrothSpeed;
	        transform.localScale = newScale;
	        if (newScale.magnitude >= FinalSize.magnitude)
	            isGrowing = false;
	    }
	}*/

    void Grow()
    {
        StartCoroutine(GrowOverTime());
    }

    IEnumerator GrowOverTime()
    {
        
        nextScale = transform.localScale + growthAmount;
        while (true)
        {
            Vector3 newScale = transform.localScale;
            float frameGrowth = GrowthSpeed * Time.deltaTime;
            newScale.x += frameGrowth;
            newScale.y += frameGrowth;
            newScale.z += frameGrowth;
            transform.localScale = newScale;
            if (newScale.magnitude >= nextScale.magnitude)
            {
                transform.localScale = nextScale;
                break;
            }
            yield return null;
        }

        if (transform.localScale.magnitude >= FinalSize.magnitude)
        {
            MessageKit.removeObserver(MessageType.OnMoveComplete, Grow);
        }
    }

    public void Activate()
    {
        MessageKit.addObserver(MessageType.OnMoveComplete, Grow);
    }

    public void Deactivate()
    {
        MessageKit.removeObserver(MessageType.OnMoveComplete, Grow);
    }
}

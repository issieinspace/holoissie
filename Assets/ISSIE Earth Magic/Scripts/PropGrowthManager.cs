using System.Collections;
using System.Collections.Generic;
using Academy.HoloToolkit.Unity;
using UnityEngine;

public class PropGrowthManager : MonoBehaviour
{

    public GameObject PropModel;
    private List<GameObject> horizontalSurfaces;
    private List<GameObject> verticalSurfaces;

    public string ExerciseName;
	// Use this for initialization
	void Start () {
	    
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTick(Hashtable args)
    {
        //TODO: Remove Magic string
        //string exerciseReady = unpackArgs(args, "exerciseName");
        //if (exerciseReady != ExerciseName)
        //    return;

        SpaceCollectionManager.Instance.GenerateItemsInWorld(PlaySpaceManager.Instance.HorizontalPlanes,
            PlaySpaceManager.Instance.VerticalPlanes, PropModel);
    }

    string unpackArgs(Hashtable table, string arg)
    {
        if (table.ContainsKey(arg))
            return table[arg].ToString();
        return null;
    }
}

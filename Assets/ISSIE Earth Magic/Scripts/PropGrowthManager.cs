using System.Collections;
using System.Collections.Generic;
using Academy.HoloToolkit.Unity;
using UnityEngine;
using Prime31.MessageKit;
using System;

public class PropGrowthManager : MonoBehaviour, IGrowable
{

    public GameObject PropModel;
    private List<GameObject> horizontalSurfaces;
    private List<GameObject> verticalSurfaces;

    public string ExerciseName;

    void CreateProp()
    {
        //TODO: Remove Magic string
        //string exerciseReady = unpackArgs(args, "exerciseName");
        //if (exerciseReady != ExerciseName)
        //    return;

        SpaceCollectionManager.Instance.GenerateItemsInWorld(PlaySpaceManager.Instance.HorizontalPlanes,
            PlaySpaceManager.Instance.VerticalPlanes, PropModel);
    }

    public void Activate()
    {
        MessageKit.addObserver(MessageType.OnStart, CreateProp);
        MessageKit.addObserver(MessageType.OnAchievement, CreateProp);
    }

    public void Deactivate()
    {
        MessageKit.removeObserver(MessageType.OnStart, CreateProp);
        MessageKit.removeObserver(MessageType.OnAchievement, CreateProp);
    }
}

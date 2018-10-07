using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorkoutCatalog
{
    public String GameName;
    public List<WorkoutDefinition> Workouts;

    public Dictionary<string, GameObject> _exercisePrefabs = new Dictionary<string, GameObject>();

 
}
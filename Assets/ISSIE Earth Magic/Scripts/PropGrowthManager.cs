using System.Collections;
using System.Collections.Generic;
using Academy.HoloToolkit.Unity;
using UnityEngine;
using Prime31.MessageKit;

public class PropGrowthManager : MonoBehaviour, ITriggerable
{

    public GameObject propPrefab;
    public float minimumSpawnDistance = 2.0f;
    public float maximumSpawnDistance = 9.0f;
    //public Transform
    private List<GameObject> horizontalSurfaces;
    private List<GameObject> verticalSurfaces;
    private List<Vector3> spawnedLocations;
    private Camera cam;
    private Vector3 viewCenter = new Vector3(0.5f, 0.5f, 0);
    private float randomDir;

    void Start()
    {
        //Random.InitState(29);
        cam = Camera.main;
        spawnedLocations = new List<Vector3>();
        randomDir = (Random.Range(0, 1) * 2) - 1;
    }

    /*void Update()
    {
        Ray camRay = cam.ViewportPointToRay(viewCenter);
        camRay.origin = new Vector3(camRay.origin.x, 0, camRay.origin.z);
        camRay.direction = new Vector3(camRay.direction.x, 0, camRay.direction.z);

        for (int i = 0; i < spawnedLocations.Count; i++)
        {
            Vector3 dirToSpawn = (spawnedLocations[i] - camRay.origin).normalized;
            Debug.DrawRay(camRay.origin,
                          dirToSpawn * 10,
                          Color.red);
            if (Vector3.Dot(camRay.direction, dirToSpawn) > 0.966f)
            {
                Debug.DrawRay(camRay.origin,
                          dirToSpawn * 10,
                          Color.green);
            }

        }
        Debug.DrawRay(camRay.origin, camRay.direction * 4, Color.blue);
    }*/

    void CreateProp()
    {
        Ray camRay = cam.ViewportPointToRay(viewCenter);
        camRay.origin = new Vector3(camRay.origin.x, 0, camRay.origin.z);
        camRay.direction = new Vector3(camRay.direction.x, 0, camRay.direction.z);

        camRay.direction = FindNearbyUnobstructedDirection(camRay);

        Vector3 spawnPoint = GetPointAlongRayWithinBounds(camRay);
        spawnedLocations.Add(spawnPoint);

        spawnPoint.y = GameManager.spacialFloorHeight;

        GameObject prop = Instantiate(propPrefab,
            spawnPoint,
            Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up));

        prop.GetComponent<PropActivator>().exerciseName = GetComponent<PropActivator>().exerciseName;
        // Compare dot products of view and camera->trees
        //    try random ray offset
        // Choose a point
        // Place prop
        // Save prop pos to list

        //SpaceCollectionManager.Instance.GenerateItemsInWorld(PlaySpaceManager.Instance.HorizontalPlanes,
        //    PlaySpaceManager.Instance.VerticalPlanes, propPrefab);

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

    private Vector3 GetPointAlongRayWithinBounds(Ray camRay)
    {
        float spawnDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);
        return camRay.origin + (camRay.direction * spawnDistance);
    }

    private Vector3 FindNearbyUnobstructedDirection(Ray camRay)
    {
        Vector3 avoidDir = new Vector3(-1, 0, -1);
        avoidDir.Normalize();
        Vector3 testDir = camRay.direction;
        if (Vector3.Dot(testDir, avoidDir) > 0.54f)
        {
            testDir = testDir * -1f;
        }
        Vector3 dirToSpawn;
        bool goodDirectionFound;

        // Get -1 or 1 randomly
        randomDir = randomDir * -1f;
        int maxIterations = 23;

        for(int iter = 0; iter < maxIterations; iter++)
        {
            // Avoid the river portion of the map
            if (Vector3.Dot(testDir, avoidDir) > 0.54f)
            {
                testDir = camRay.direction;
                randomDir = randomDir * -1;
            }

            goodDirectionFound = true;
            for (int i = 0; i < spawnedLocations.Count; i++)
            {
                dirToSpawn = (spawnedLocations[i] - camRay.origin).normalized;
                var v = Vector3.Dot(testDir, dirToSpawn);
                if (Vector3.Dot(testDir, dirToSpawn) > 0.966f)
                {
                    goodDirectionFound = false;
                    break;
                }
            }
            if (goodDirectionFound)
            {
                return testDir;
            }
            else
            {
                testDir = Quaternion.Euler(0, 4.3f * randomDir, 0) * testDir;
            }
        }
        return camRay.direction;
    }
}

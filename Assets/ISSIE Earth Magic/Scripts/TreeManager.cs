using System.Collections;
using System.Collections.Generic;
using Academy.HoloToolkit.Unity;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

    public GameObject TreeModel;
    private List<GameObject> horizontalSurfaces;
    private List<GameObject> verticalSurfaces;

    public string ExerciseName;
	// Use this for initialization
	void Start () {
	    // Register for the MakePlanesComplete event.
	    SurfaceMeshesToPlanes.Instance.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Handler for the SurfaceMeshesToPlanes MakePlanesComplete event.
    /// </summary>
    /// <param name="source">Source of the event.</param>
    /// <param name="args">Args for the event.</param>
    private void SurfaceMeshesToPlanes_MakePlanesComplete(object source, System.EventArgs args)
    {
        // Collection of floor and table planes that we can use to set horizontal items on.
        horizontalSurfaces = new List<GameObject>();

        // Collection of wall planes that we can use to set vertical items on.
        verticalSurfaces = new List<GameObject>();

        // 3.a: Get all floor and table planes by calling
        // SurfaceMeshesToPlanes.Instance.GetActivePlanes().
        // Assign the result to the 'horizontal' list.
        horizontalSurfaces = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Table | PlaneTypes.Floor);

        // Check to see if we have enough horizontal planes (minimumFloors)
        // and vertical planes (minimumWalls), to set holograms on in the world.
        if (horizontalSurfaces.Count >= 1)
        {
            // We have enough floors and walls to place our holograms on...

            // 3.a: Let's reduce our triangle count by removing triangles
            // from SpatialMapping meshes that intersect with our active planes.
            // Call RemoveVertices().
            // Pass in all activePlanes found by SurfaceMeshesToPlanes.Instance.
            RemoveVertices(SurfaceMeshesToPlanes.Instance.ActivePlanes);


            // 3.a: We are all done processing the mesh, so we can now
            // initialize a collection of Placeable holograms in the world
            // and use horizontal/vertical planes to set their starting positions.
            // Call SpaceCollectionManager.Instance.GenerateItemsInWorld().
            // Pass in the lists of horizontal and vertical planes that we found earlier.
            //SpaceCollectionManager.Instance.GenerateItemsInWorld(horizontalSurfaces, verticalSurfaces, TreeModel);
        }
        else
        {
        }
    }
    /// <summary>
    /// Removes triangles from the spatial mapping surfaces.
    /// </summary>
    /// <param name="boundingObjects"></param>
    private void RemoveVertices(IEnumerable<GameObject> boundingObjects)
    {
        RemoveSurfaceVertices removeVerts = RemoveSurfaceVertices.Instance;
        if (removeVerts != null && removeVerts.enabled)
        {
            removeVerts.RemoveSurfaceVerticesWithinBounds(boundingObjects);
        }
    }

    void OnReady(Hashtable args)
    {
        //TODO: Remove Magic string
        string exerciseReady = unpackArgs(args, "exerciseName");
        if (exerciseReady != ExerciseName)
            return;

        SpaceCollectionManager.Instance.GenerateItemsInWorld(horizontalSurfaces, verticalSurfaces, TreeModel);
    }

    string unpackArgs(Hashtable table, string arg)
    {
        if (table.ContainsKey(arg))
            return table[arg].ToString();
        return null;
    }
}

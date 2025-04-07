using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class FloorNavMeshManager : MonoBehaviour
{

    // automatically scales the navmesh surface to the floor
    private NavMeshSurface navMeshSurface;
    public float sizeMultiplier = 1.0f; // Multiplier for NavMeshSurface size
    public bool accountForParentScale = true; // Whether to account for parent scale

    void Start()
    {
        // Get the NavMeshSurface component
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface component is missing on the floor!");
            return;
        }

        // Adjust the NavMeshSurface's center and size based on the floor's dimensions
        AdjustNavMeshSurfaceToFloor();

        // Rebuild the NavMesh for the floor
        navMeshSurface.BuildNavMesh();
        
        // Print the world position and size of the NavMeshSurface
        Vector3 worldCenter = transform.TransformPoint(navMeshSurface.center);
        Vector3 worldSize = navMeshSurface.size;
        worldSize.x *= transform.lossyScale.x;
        worldSize.y *= transform.lossyScale.y;
        worldSize.z *= transform.lossyScale.z;
        
        Debug.Log($"NavMeshSurface world position for {gameObject.name}: center={worldCenter}, size={worldSize}");
        
        // Print the floor name to identify Floor1
        if (gameObject.name.ToLower().Contains("floor1"))
        {
            Debug.Log($"FLOOR1 NAVMESH INFO: center={worldCenter}, size={worldSize}");
        }
    }

    // Adjust the NavMeshSurface's center and size based on the floor's dimensions
    private void AdjustNavMeshSurfaceToFloor()
    {
        // Find all renderers in the floor to determine its bounds
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning($"No renderers found in the floor {gameObject.name}. Using default NavMeshSurface settings.");
            return;
        }

        // Calculate the combined bounds of all renderers
        Bounds floorBounds = new Bounds();
        bool firstBounds = true;
        foreach (Renderer renderer in renderers)
        {
            if (firstBounds)
            {
                floorBounds = renderer.bounds;
                firstBounds = false;
            }
            else
            {
                floorBounds.Encapsulate(renderer.bounds);
            }
        }

        // Print the floor bounds
        Debug.Log($"Floor bounds for {gameObject.name}: center={floorBounds.center}, size={floorBounds.size}");
        
        // If this is Floor1, print additional info
        if (gameObject.name.ToLower().Contains("floor1"))
        {
            Debug.Log($"FLOOR1 BOUNDS INFO: center={floorBounds.center}, size={floorBounds.size}");
        }

        // Convert world bounds to local bounds relative to the floor
        Vector3 localCenter = transform.InverseTransformPoint(floorBounds.center);
        
        // Calculate local size
        Vector3 localSize = floorBounds.size;
        
        // Account for parent scale if needed
        if (accountForParentScale)
        {
            localSize.x /= transform.lossyScale.x;
            localSize.y /= transform.lossyScale.y;
            localSize.z /= transform.lossyScale.z;
            
            Debug.Log($"Adjusted for parent scale. Floor scale: {transform.lossyScale}");
        }
        
        // Apply size multiplier if needed
        localSize *= sizeMultiplier;
        
        // Set the NavMeshSurface's center and size
        navMeshSurface.center = localCenter;
        navMeshSurface.size = localSize;
        
        Debug.Log($"NavMeshSurface adjusted for floor {gameObject.name}: center={localCenter}, size={localSize}");
    }

    // Public method to rebuild the NavMesh at any time
    public void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log($"NavMesh has been rebuilt for floor: {gameObject.name}");
        }
    }
}
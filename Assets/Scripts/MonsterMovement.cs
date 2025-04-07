using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class MonsterMovement : MonoBehaviour
{
    private Transform player; // xr origin
    private Transform currentFloor;
    private int monsterFloorNumber;
    private NavMeshAgent navMeshAgent;
    private NavMeshSurface navMeshSurface;
    private bool isNavMeshAgentEnabled = false;
    private bool useDirectMovement = false;
    private float moveSpeed = 0.25f;
    private float rotationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the monster!");
            return;
        }
        
        // Disable the NavMeshAgent initially
        navMeshAgent.enabled = false;

        useDirectMovement = true;

        // Find the XR Origin player object
        player = GameObject.Find("XR Origin (XR Rig)")?.transform;
        
        if (player == null)
        {
            Debug.LogError("Could not find XR Origin (XR Rig) player object!");
            return;
        }

        // Get the floor this monster is on
        currentFloor = transform.parent;
        if (currentFloor == null)
        {
            Debug.LogError("Monster has no parent floor!");
            return;
        }

        // Get the floor number directly from the parent's name
        string floorName = currentFloor.name.ToLower();
        if (floorName.StartsWith("floor1"))
        {
            monsterFloorNumber = 1;
        }
        else if (floorName.StartsWith("floor2"))
        {
            monsterFloorNumber = 2;
        }
        else if (floorName.StartsWith("floor3"))
        {
            monsterFloorNumber = 3;
        }
        else
        {
            Debug.LogError($"Invalid floor name: {currentFloor.name}");
            return;
        }
        
        // Print initial debug info
        Debug.Log($"Monster '{gameObject.name}' initialized on floor {monsterFloorNumber}. Player floor: {FloorManager.CurrentFloorNumber}");
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        // Try to enable NavMeshAgent if not already enabled
        if (!isNavMeshAgentEnabled)
        {
            TryEnableNavMeshAgent();
        }
    }

    // Try to enable the NavMeshAgent
    private void TryEnableNavMeshAgent()
    {
        // Find the NavMeshSurface on the current floor
        navMeshSurface = currentFloor.GetComponent<NavMeshSurface>();
        
        if (navMeshSurface == null)
        {
            Debug.LogError("No NavMeshSurface found on the current floor!");
            return;
        }
        
        // Check if NavMesh is valid at the original position
        // NavMeshHit hit;
        
        // bool isValidPosition = NavMesh.SamplePosition(positionToCheck, out hit, 5.0f, NavMesh.AllAreas);
        
        // if (isValidPosition) {
        //     // Position the monster at the desired height
        //     Vector3 newPosition = hit.position;
            
        //     // Enable the NavMeshAgent
        //     navMeshAgent.enabled = true;
        //     isNavMeshAgentEnabled = true;
            
        //     Debug.Log($"Enabled NavMeshAgent for monster: {gameObject.name}");
        // }
        // else
        // {
        //     Debug.LogWarning($"No valid NavMesh position found for monster: {gameObject.name}");
        //     return; // Don't enable the NavMeshAgent if we can't find a valid position
        // }
    }

    // Update is called once per frame
    void Update()
    {   
        // Only move if the monster is on the same floor as the player
        if (monsterFloorNumber == FloorManager.CurrentFloorNumber)
        {
            // Debug info every few seconds
            if (player != null && navMeshAgent != null && navMeshAgent.enabled)
            {    
                // Found a valid position, set it as the destination
                navMeshAgent.SetDestination(player.position);
            }
            // If using direct movement, move directly towards the player
            else if (useDirectMovement)
            {
                if (player != null)
                {
                    // Calculate direction to player
                    Vector3 direction = (player.position - transform.position).normalized;
                    
                    // Rotate towards player
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    
                    // Move towards player
                    transform.position += direction * moveSpeed * Time.deltaTime;
                }
            }
        }
    }
}

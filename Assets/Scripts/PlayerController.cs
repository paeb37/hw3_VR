using UnityEngine;

// PlayerController handles player interactions with monsters and bosses in the VR environment
// It manages collision detection, death sounds, and respawn mechanics

public class PlayerController : MonoBehaviour
{
    // Reference to the XR Origin which represents the player's position in VR space
    private Transform xrOrigin; // Will be found at runtime
    
    // Audio source for playing death sounds when player collides with monsters
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource component

    // Initialize component references and find the XR Origin in the scene
    private void Start()
    {
        // Find the XR Origin (XR Rig) in the scene - this is the root of the VR player
        GameObject xrOriginObj = GameObject.Find("XR Origin (XR Rig)");
        if (xrOriginObj != null)
        {
            xrOrigin = xrOriginObj.transform;
        }
    }

    // This method is automatically called by Unity when the player's collider hits another collider
    // It determines what type of object was hit and handles the appropriate response
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if we hit a Boss - this takes priority over regular monsters
        if (hit.gameObject.CompareTag("Boss")) // visuals
        {
            // Bosses always send the player to Floor3 (the boss floor)
            TeleportToFloor("Floor3");
        }
        // If not a boss, check if it's a regular monster
        else if (hit.gameObject.CompareTag("Monster")) // visuals
        {
            // Get the monster GameObject (either the hit object itself or its parent)
            // Transform monsterTransform = hit.gameObject.CompareTag("Monster") ? hit.transform : hit.transform.parent;
            HandleMonsterCollision(hit.transform.parent); // should be the parent transform
        }
    }

    // Determines which floor the player should respawn on based on the monster's location
    // This ensures the player respawns on the same floor where they encountered the monster
    private void HandleMonsterCollision(Transform monsterTransform)
    {
        // Get the monster's parent to determine which floor we're on
        Transform monsterParent = monsterTransform.parent;
        
        if (monsterParent != null)
        {
            // Check which floor we're on and respawn there
            // This creates a consistent experience where players stay on their current floor
            if (monsterParent.name.Contains("Floor1")) TeleportToFloor("Floor1");
            else if (monsterParent.name.Contains("Floor2")) TeleportToFloor("Floor2");
            else if (monsterParent.name.Contains("Floor3")) TeleportToFloor("Floor3");
        }
    }

    // Handles the actual teleportation process, including playing death sounds and updating UI
    // This is the central method for player respawning throughout the game
    private void TeleportToFloor(string floorName)
    {
        // Play death sound if available - provides audio feedback for player death
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Show player died text - provides visual feedback for player death
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPlayerDiedText();
        }

        // Find the floor GameObject in the scene hierarchy
        GameObject floor = GameObject.Find(floorName);
        if (floor != null)
        {
            // Find the player spawn point within this floor
            // Spawn points are tagged with "Player" for easy identification
            GameObject spawnPoint = null;
            foreach (Transform child in floor.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag("Player"))
                {
                    spawnPoint = child.gameObject;
                    break;
                }
            }

            // If we found both a spawn point and the XR Origin, teleport the player
            if (spawnPoint != null && xrOrigin != null)
            {
                // Teleport XR Origin to spawn point - this moves the entire VR player
                xrOrigin.position = spawnPoint.transform.position;
                xrOrigin.rotation = spawnPoint.transform.rotation;
            }
        }
    }
}

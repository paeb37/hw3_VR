using UnityEngine;

public class PlayerMonsterCollisionHandler : MonoBehaviour
{
    private Transform xrOrigin; // Will be found at runtime
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Find the XR Origin (XR Rig) in the scene
        GameObject xrOriginObj = GameObject.Find("XR Origin (XR Rig)");
        if (xrOriginObj != null)
        {
            xrOrigin = xrOriginObj.transform;
            Debug.Log("✅ Found XR Origin (XR Rig) in scene");
        }
        else
        {
            Debug.LogWarning("⚠️ Could not find XR Origin (XR Rig) in scene");
        }
    }

    // this method is the one that handles the collision with the monster
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log($"🔍 Collision detected with: {hit.gameObject.name}, Tag: {hit.gameObject.tag}");
        
        // Add detailed debug information about the hit object and its hierarchy
        // Debug.Log($"📋 Hit object details:");
        // Debug.Log($"- Name: {hit.gameObject.name}");
        // Debug.Log($"- Tag: {hit.gameObject.tag}");
        // Debug.Log($"- Has Collider: {hit.gameObject.GetComponent<Collider>() != null}");
        // Debug.Log($"- Parent: {hit.transform.parent?.name ?? "No parent"}");
        // Debug.Log($"- Parent Tag: {hit.transform.parent?.tag ?? "No parent tag"}");
        // Debug.Log($"- Parent Has Collider: {hit.transform.parent?.GetComponent<Collider>() != null}");

        // Check if we hit a Boss - this takes priority
        if (hit.gameObject.CompareTag("Boss")) // visuals
        {
            Debug.Log("👑 Boss collision detected - Sending player to Floor3");
            TeleportToFloor("Floor3");
        }
        // If not a boss, check if it's a regular monster
        else if (hit.gameObject.CompareTag("Monster")) // visuals
        {
            Debug.Log("💥 Monster collision detected");
            // Get the monster GameObject (either the hit object itself or its parent)
            // Transform monsterTransform = hit.gameObject.CompareTag("Monster") ? hit.transform : hit.transform.parent;
            HandleMonsterCollision(hit.transform.parent); // should be the parent transform
        }
    }

    private void HandleMonsterCollision(Transform monsterTransform)
    {
        // Get the monster's parent to determine which floor we're on
        Transform monsterParent = monsterTransform.parent;
        
        if (monsterParent != null)
        {
            Debug.Log($"🏢 Monster's parent: {monsterParent.name}");

            // Check which floor we're on and respawn there
            if (monsterParent.name.Contains("Floor1")) TeleportToFloor("Floor1");
            else if (monsterParent.name.Contains("Floor2")) TeleportToFloor("Floor2");
            else if (monsterParent.name.Contains("Floor3")) TeleportToFloor("Floor3");
            else Debug.LogWarning("⚠️ Monster's parent does not contain a valid floor name");
        }
        else
        {
            Debug.LogWarning("⚠️ Monster has no parent transform");
        }
    }

    private void TeleportToFloor(string floorName)
    {
        Debug.Log($"🔄 Attempting to teleport to {floorName}");

        // Play death sound if available
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Debug.Log("💀 Playing death sound");
        }
        else if (audioSource == null)
        {
            Debug.LogWarning("⚠️ AudioSource component not assigned!");
        }
        else if (audioSource.clip == null)
        {
            Debug.LogWarning("⚠️ No audio clip assigned to AudioSource!");
        }

        // Show player died text
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPlayerDiedText();
            Debug.Log("💀 Showing player died text");
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager instance not found!");
        }

        // Find the floor GameObject
        GameObject floor = GameObject.Find(floorName);
        if (floor != null)
        {
            // Find the player spawn point within this floor
            GameObject spawnPoint = null;
            foreach (Transform child in floor.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag("Player"))
                {
                    spawnPoint = child.gameObject;
                    Debug.Log($"✅ Found spawn point in {floorName}: {spawnPoint.name}");
                    break;
                }
            }

            if (spawnPoint != null && xrOrigin != null)
            {
                // Teleport XR Origin to spawn point
                xrOrigin.position = spawnPoint.transform.position;
                xrOrigin.rotation = spawnPoint.transform.rotation;
                Debug.Log($"🎮 XR Origin respawned at {floorName} spawn point: {spawnPoint.transform.position}");
            }
            else
            {
                if (spawnPoint == null)
                    Debug.LogWarning($"⚠️ No spawn point (Player tag) found in {floorName}");
                if (xrOrigin == null)
                    Debug.LogWarning("⚠️ XR Origin reference lost");
            }
        }
        else
        {
            Debug.LogWarning($"⚠️ Could not find {floorName} GameObject");
        }
    }
}

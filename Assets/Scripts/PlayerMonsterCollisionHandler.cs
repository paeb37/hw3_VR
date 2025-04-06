using UnityEngine;

public class PlayerMonsterCollisionHandler : MonoBehaviour
{
    private Transform xrOrigin; // Will be found at runtime

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log($"🔍 Collision detected with: {hit.gameObject.name}, Tag: {hit.gameObject.tag}");

        // Check if we hit a Boss - this takes priority
        if (hit.gameObject.CompareTag("Boss") || hit.transform.parent.gameObject.CompareTag("Boss"))
        {
            Debug.Log("👑 Boss collision detected - Sending player to Floor3");
            TeleportToFloor("Floor3");
        }
        // If not a boss, check if it's a regular monster
        else if (hit.gameObject.CompareTag("Monster")  || hit.transform.parent.gameObject.CompareTag("Monster"))
        {
            Debug.Log("💥 Monster collision detected");
            // Get the monster GameObject (either the hit object itself or its parent)
            Transform monsterTransform = hit.gameObject.CompareTag("Monster") ? hit.transform : hit.transform.parent;
            HandleMonsterCollision(monsterTransform);
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

        // Find the floor GameObject
        GameObject floor = GameObject.Find(floorName);
        if (floor != null)
        {
            // Find the player spawn point within this floor
            GameObject spawnPoint = null;
            foreach (Transform child in floor.GetComponentsInChildren<Transform>())
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

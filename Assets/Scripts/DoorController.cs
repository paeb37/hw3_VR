using UnityEngine;

public class DoorController : MonoBehaviour
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
        
        // Debug door hierarchy
        Debug.Log($"🚪 Door hierarchy: {gameObject.name} -> Parent: {transform.parent?.name} -> Grandparent: {transform.parent?.parent?.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🔍 OnTriggerEnter called with object: {other.gameObject.name}, Tag: {other.tag}");
        
        // Debug statement to log when a key collides with the door
        if (other.CompareTag("Key"))
        {
            Debug.Log($"🔑 Key collided with door: {gameObject.name}");
        }
        
        // Debug statement to log when a controller collides with the door
        if (other.CompareTag("Controller"))
        {
            Debug.Log($"🎮 Controller collided with door: {gameObject.name}");
        }
        
        // Check if the colliding object has the Key tag AND if door's parent contains "Floor1"
        bool isKey = other.CompareTag("Key");
        bool hasParent = transform.parent != null;
        bool hasGrandparent = transform.parent?.parent != null;
        string grandparentName = hasGrandparent ? transform.parent.parent.name : "null";
        bool isFloor1 = hasGrandparent && grandparentName.Contains("Floor1");
        
        Debug.Log($"🔍 Collision check: isKey={isKey}, hasParent={hasParent}, hasGrandparent={hasGrandparent}, grandparentName={grandparentName}, isFloor1={isFloor1}");
        
        // floor 1
        if (isKey && hasGrandparent && isFloor1)
        {
            Debug.Log("✅ Key collision conditions met, attempting teleport");
            
            // Find the Floor2 GameObject
            GameObject floor2 = GameObject.Find("Floor2");
            if (floor2 != null)
            {
                Debug.Log("✅ Found Floor2 object");
                
                // Find the player within Floor2
                GameObject player = null;
                foreach (Transform child in floor2.GetComponentsInChildren<Transform>())
                {
                    if (child.CompareTag("Player"))
                    {
                        player = child.gameObject;
                        Debug.Log($"✅ Found player in Floor2: {player.name}");
                        break;
                    }
                }

                if (player != null && xrOrigin != null)
                {
                    // Set XR Origin's world position and rotation to the Floor2 player's position
                    xrOrigin.position = player.transform.position;
                    xrOrigin.rotation = player.transform.rotation;
                    Debug.Log($"🎮 XR Origin teleported to Floor2 player position: {player.transform.position}");

                    // Optionally destroy the key
                    Destroy(other.gameObject);
                    Debug.Log("🗑️ Key destroyed");
                }
                else
                {
                    if (player == null)
                        Debug.LogWarning("⚠️ No object with Player tag found in Floor2");
                    if (xrOrigin == null)
                        Debug.LogWarning("⚠️ XR Origin not found in scene");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Could not find GameObject named 'Floor2'");
            }
        }
        else
        {
            Debug.Log("❌ Key collision conditions not met");
        }


        // floor 2
        bool isPlayer = other.CompareTag("Controller"); // controller hits the door
        bool isFloor2 = hasGrandparent && grandparentName.Contains("Floor2");
        
        Debug.Log($"🔍 Floor2 collision check: isPlayer={isPlayer}, hasParent={hasParent}, hasGrandparent={hasGrandparent}, grandparentName={grandparentName}, isFloor2={isFloor2}");
        
        if (isPlayer && hasGrandparent && isFloor2)
        {
            Debug.Log("✅ Player collision conditions met for Floor2, attempting teleport");
            
            // Find the Floor3 GameObject
            GameObject floor3 = GameObject.Find("Floor3");
            if (floor3 != null)
            {
                Debug.Log("✅ Found Floor3 object");
                
                // Find the player within Floor3
                GameObject player = null;
                foreach (Transform child in floor3.GetComponentsInChildren<Transform>())
                {
                    if (child.CompareTag("Player"))
                    {
                        player = child.gameObject;
                        Debug.Log($"✅ Found player in Floor3: {player.name}");
                        break;
                    }
                }

                if (player != null && xrOrigin != null)
                {
                    // Set XR Origin's world position and rotation to the Floor3 player's position
                    xrOrigin.position = player.transform.position;
                    xrOrigin.rotation = player.transform.rotation;
                    Debug.Log($"🎮 XR Origin teleported to Floor3 player position: {player.transform.position}");
                }
                else
                {
                    if (player == null)
                        Debug.LogWarning("⚠️ No object with Player tag found in Floor3");
                    if (xrOrigin == null)
                        Debug.LogWarning("⚠️ XR Origin not found in scene");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Could not find GameObject named 'Floor3'");
            }
        }
        else
        {
            Debug.Log("❌ Controller collision conditions not met for Floor2");
        }
    }
}

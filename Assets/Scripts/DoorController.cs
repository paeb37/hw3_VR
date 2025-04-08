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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the Key tag AND if door's parent contains "Floor1"
        bool isKey = other.CompareTag("Key");
        bool hasGrandparent = transform.parent?.parent != null;
        string grandparentName = hasGrandparent ? transform.parent.parent.name : "null";
        bool isFloor1 = hasGrandparent && grandparentName.Contains("Floor1");
        
        // floor 1
        if (isKey && hasGrandparent && isFloor1)
        {
            // Find the Floor2 GameObject
            GameObject floor2 = GameObject.Find("Floor2");
            if (floor2 != null)
            {
                // Find the player within Floor2
                GameObject player = null;

                // set true to include inactive children
                foreach (Transform child in floor2.GetComponentsInChildren<Transform>(true))
                {
                    if (child.CompareTag("Player"))
                    {
                        player = child.gameObject;
                        break;
                    }
                }

                if (player != null && xrOrigin != null)
                {
                    // Set XR Origin's world position and rotation to the Floor2 player's position
                    xrOrigin.position = player.transform.position;
                    xrOrigin.rotation = player.transform.rotation;

                    // Update FloorManager to track that player is now on Floor 2
                    FloorManager.SetCurrentFloor(2);

                    // Optionally destroy the key
                    Destroy(other.gameObject);
                }
            }
        }

        // floor 2
        bool isPlayer = other.CompareTag("Controller"); // controller hits the door
        bool isFloor2 = hasGrandparent && grandparentName.Contains("Floor2");
        
        if (isPlayer && hasGrandparent && isFloor2)
        {
            // Find the Floor3 GameObject
            GameObject floor3 = GameObject.Find("Floor3");
            if (floor3 != null)
            {
                // Find the player within Floor3
                GameObject player = null;
                
                // set true to include inactive children
                foreach (Transform child in floor3.GetComponentsInChildren<Transform>(true))
                {
                    if (child.CompareTag("Player"))
                    {
                        player = child.gameObject;
                        break;
                    }
                }

                if (player != null && xrOrigin != null)
                {
                    // Set XR Origin's world position and rotation to the Floor3 player's position
                    xrOrigin.position = player.transform.position;
                    xrOrigin.rotation = player.transform.rotation;
                    
                    // Update FloorManager to track that player is now on Floor 3
                    FloorManager.SetCurrentFloor(3);
                }
            }
        }
    }
}

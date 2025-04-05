using UnityEngine;

public class MonsterCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab; // Reference to the key prefab
    [SerializeField] private float keyHeightOffset = 0.5f; // Height above the floor to spawn the key
    private bool keySpawned = false; // Flag to prevent multiple key spawns

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has any of the specified tags
        if (other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2"))
        {
            Debug.Log("Trigger collision detected with Controller, Weapon1, or Weapon2. Destroying monster.");
            
            // Store monster's position and rotation before destroying
            Vector3 monsterPosition = transform.position;
            Quaternion monsterRotation = transform.rotation;
            
            // Find the floor object (parent of the parent of this object)
            Transform floorTransform = transform.parent.parent;
            Debug.Log($"Floor transform found: {floorTransform != null}, Name: {(floorTransform != null ? floorTransform.name : "null")}");
            
            // Check if parent contains "Floor1" in its name
            if (transform.parent.parent != null && transform.parent.parent.name.Contains("Floor1") && keyPrefab != null && floorTransform != null && !keySpawned)
            {
                Debug.Log("All conditions met for key spawning. Spawning key...");
                
                // Adjust the Y position to be above the floor
                Vector3 keyPosition = new Vector3(monsterPosition.x, monsterPosition.y + keyHeightOffset, monsterPosition.z);
                
                // Instantiate the key at the adjusted position
                GameObject newKey = Instantiate(keyPrefab, keyPosition, monsterRotation);
                // Set the key as a child of the floor object
                newKey.transform.SetParent(floorTransform);
                Debug.Log($"Key spawned successfully: {newKey.name} at position {keyPosition}");
                
                // Set the flag to prevent multiple spawns
                keySpawned = true;
            }
            else
            {
                // Check each condition individually and log which one failed
                if (transform.parent.parent == null)
                {
                    Debug.Log("Key spawning failed: Monster's parent.parent is null");
                }
                else if (!transform.parent.parent.name.Contains("Floor1"))
                {
                    Debug.Log($"Key spawning failed: Monster's parent.parent name '{transform.parent.parent.name}' does not contain 'Floor1'");
                }
                else if (keyPrefab == null)
                {
                    Debug.Log("Key spawning failed: Key prefab is not assigned in the Inspector");
                }
                else if (floorTransform == null)
                {
                    Debug.Log("Key spawning failed: Floor transform is null");
                }
                else if (keySpawned)
                {
                    Debug.Log("Key spawning failed: Key was already spawned");
                }
            }
            
            // Destroy the monster
            Destroy(transform.parent.gameObject);
        }
    }

    /* private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has any of the specified tags
        if (collision.gameObject.CompareTag("Controller") || 
            collision.gameObject.CompareTag("Weapon1") || 
            collision.gameObject.CompareTag("Weapon2"))
        {
            Debug.Log("Collision detected with Controller, Weapon1, or Weapon2. Destroying monster.");
            
            // Store monster's position and rotation before destroying
            Vector3 monsterPosition = transform.position;
            Quaternion monsterRotation = transform.rotation;
            
            // Find the floor object (parent of the parent of this object)
            Transform floorTransform = transform.parent.parent;
            Debug.Log($"Floor transform found: {floorTransform != null}, Name: {(floorTransform != null ? floorTransform.name : "null")}");
            
            // Check if parent contains "Floor1" in its name
            if (transform.parent.parent != null && transform.parent.parent.name.Contains("Floor1") && keyPrefab != null && floorTransform != null)
            {
                Debug.Log("All conditions met for key spawning. Spawning key...");
                
                // Adjust the Y position to be above the floor
                Vector3 keyPosition = new Vector3(monsterPosition.x, monsterPosition.y + keyHeightOffset, monsterPosition.z);
                
                // Instantiate the key at the adjusted position
                GameObject newKey = Instantiate(keyPrefab, keyPosition, monsterRotation);
                // Set the key as a child of the floor object
                newKey.transform.SetParent(floorTransform);
                Debug.Log($"Key spawned successfully: {newKey.name} at position {keyPosition}");
            }
            else
            {
                // Check each condition individually and log which one failed
                if (transform.parent.parent == null)
                {
                    Debug.Log("Key spawning failed: Monster's parent.parent is null");
                }
                else if (!transform.parent.parent.name.Contains("Floor1"))
                {
                    Debug.Log($"Key spawning failed: Monster's parent.parent name '{transform.parent.parent.name}' does not contain 'Floor1'");
                }
                else if (keyPrefab == null)
                {
                    Debug.Log("Key spawning failed: Key prefab is not assigned in the Inspector");
                }
                else if (floorTransform == null)
                {
                    Debug.Log("Key spawning failed: Floor transform is null");
                }
            }
            
            // Destroy the monster
            Destroy(transform.parent.gameObject);
        }
    } */
}

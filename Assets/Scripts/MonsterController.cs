using UnityEngine;
using TMPro; // Add TextMeshPro import

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab; // Reference to the key prefab
    [SerializeField] private float keyHeightOffset = 0.5f; // Height above the floor to spawn the key
    private bool keySpawned = false; // Flag to prevent multiple key spawns

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has any of the specified tags
        if (other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2"))
        {
            // Show kill text using UIManager
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowKillText();
            }
            
            // Play death sound using AudioManager
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMonsterDeathSound();
            }
            
            // Store monster's position and rotation before destroying
            Vector3 monsterPosition = transform.position;
            Quaternion monsterRotation = transform.rotation;
            
            // Find the floor object (parent of the parent of this object)
            Transform floorTransform = transform.parent.parent;
            
            // Check if parent contains "Floor1" in its name
            if (transform.parent.parent != null && transform.parent.parent.name.Contains("Floor1") && keyPrefab != null && floorTransform != null && !keySpawned)
            {
                // Adjust the Y position to be above the floor
                Vector3 keyPosition = new Vector3(monsterPosition.x, monsterPosition.y + keyHeightOffset, monsterPosition.z);
                
                // Instantiate the key at the adjusted position
                GameObject newKey = Instantiate(keyPrefab, keyPosition, monsterRotation);
                // Set the key as a child of the floor object
                newKey.transform.SetParent(floorTransform);
                
                // Set the flag to prevent multiple spawns
                keySpawned = true;
            }
            
            // Destroy the monster
            Destroy(transform.parent.gameObject);
        }
    }
}

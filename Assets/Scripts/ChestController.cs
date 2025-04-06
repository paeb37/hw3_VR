using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private GameObject weapon1Prefab;
    [SerializeField] private GameObject weapon2Prefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called with object: {other.gameObject.name} with tag: {other.tag}");
        
        // Check if the colliding object is the right controller
        if (other.CompareTag("Controller"))
        {
            Debug.Log("Controller tag detected, proceeding with chest opening");
            
            // Store the chest's position before destroying it
            Vector3 chestPosition = transform.position;
            Quaternion chestRotation = transform.rotation;
            
            // Find the floor object (parent of the parent of this object)
            Transform floorTransform = transform.parent.parent;
            if (floorTransform == null)
            {
                Debug.LogError("Could not find floor object in hierarchy!");
                return;
            }
            
            // Randomly choose between Weapon1 and Weapon2
            string chosenTag = (Random.value < 0.5f) ? "Weapon1" : "Weapon2";
            Debug.Log($"Chosen weapon tag: {chosenTag}");
            
            GameObject weaponToSpawn = null;
            
            // Get the reference object based on the chosen tag
            if (chosenTag == "Weapon1" && weapon1Prefab != null)
            {
                weaponToSpawn = weapon1Prefab;
                Debug.Log("Weapon1 prefab found and selected");
            }
            else if (chosenTag == "Weapon2" && weapon2Prefab != null)
            {
                weaponToSpawn = weapon2Prefab;
                Debug.Log("Weapon2 prefab found and selected");
            }
            
            // Spawn the chosen weapon
            if (weaponToSpawn != null)
            {
                GameObject newWeapon = Instantiate(weaponToSpawn, chestPosition, chestRotation);
                newWeapon.tag = chosenTag; // Ensure the new weapon has the same tag
                newWeapon.transform.SetParent(floorTransform); // Parent to the floor object
                Debug.Log($"Spawned new weapon: {newWeapon.name} with tag: {chosenTag} as child of floor");
            }
            else
            {
                Debug.LogWarning($"No weapon prefab assigned for tag: {chosenTag}");
            }
            
            Debug.Log("Destroying chest parent...");
            // Destroy the parent object instead of this object
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log($"Collision detected but object tag '{other.tag}' is not 'Controller'");
        }
    }
}

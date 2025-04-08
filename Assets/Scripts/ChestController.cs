using UnityEngine;

public class ChestController : MonoBehaviour
{
    // these are the variables for the chest's weapons
    [SerializeField] private GameObject weapon1Prefab;
    [SerializeField] private GameObject weapon2Prefab;

    // this is the function for the chest's weapons
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the right controller
        if (other.CompareTag("Controller"))
        {
            // Store the chest's position before destroying it
            Vector3 chestPosition = transform.position;
            Quaternion chestRotation = transform.rotation;
            
            // Find the floor object (parent of the parent of this object)
            Transform floorTransform = transform.parent.parent;
            if (floorTransform == null)
            {
                return;
            }
            
            // Randomly choose between Weapon1 and Weapon2 with 50/50 chance
            string chosenTag = (Random.value < 0.5f) ? "Weapon1" : "Weapon2";
            
            // Initialize weapon reference as null until we determine which one to spawn
            GameObject weaponToSpawn = null;
            
            // Get the reference object based on the chosen tag
            if (chosenTag == "Weapon1" && weapon1Prefab != null)
            {
                weaponToSpawn = weapon1Prefab;
            }
            else if (chosenTag == "Weapon2" && weapon2Prefab != null)
            {
                weaponToSpawn = weapon2Prefab;
            }
            
            // Spawn the chosen weapon at the chest's position
            if (weaponToSpawn != null)
            {
                // Create the new weapon instance with the chest's position and rotation
                GameObject newWeapon = Instantiate(weaponToSpawn, chestPosition, chestRotation);
                newWeapon.tag = chosenTag; // Ensure the new weapon has the same tag
                newWeapon.transform.SetParent(floorTransform); // Parent to the floor object
            }
            
            // Clean up by destroying the chest after spawning the weapon
            Destroy(transform.parent.gameObject);
        }
    }
}

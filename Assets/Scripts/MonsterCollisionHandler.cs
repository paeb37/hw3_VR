using UnityEngine;

public class MonsterCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has any of the specified tags
        if (/* other.CompareTag("Controller") || */ other.CompareTag("Weapon1") || other.CompareTag("Weapon2"))
        {
            Debug.Log("Trigger collision detected with Weapon1 or Weapon2. Destroying monster.");
            // Destroy the monster
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has any of the specified tags
        if (/* collision.gameObject.CompareTag("Controller") || */ 
            collision.gameObject.CompareTag("Weapon1") || 
            collision.gameObject.CompareTag("Weapon2"))
        {
            Debug.Log("Collision detected with Weapon1 or Weapon2. Destroying monster.");
            // Destroy the monster
            Destroy(transform.parent.gameObject);
        }
    }
}

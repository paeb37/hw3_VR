using UnityEngine;

public class MonsterCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Controller" tag
        if (other.CompareTag("Controller"))
        {
            Debug.Log("Trigger collision detected with Controller. Destroying monster.");
            // Destroy the monster
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the "Controller" tag
        if (collision.gameObject.CompareTag("Controller"))
        {
            Debug.Log("Collision detected with Controller. Destroying monster.");
            // Destroy the monster
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class BossController : MonoBehaviour
{

    // these are the variables for the boss's health
    private int hitsRequired = 2;
    private int currentHits = 0;
    private bool canBeHit = true;
    private bool needsExit = false;

    private void Start()
    {
        // nothing to do here
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBeHit && !needsExit && (other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2")))
        {
            currentHits++;

            if (currentHits >= hitsRequired)
            {
                // Show victory text before destroying the boss
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowVictoryMessage();
                }

                // Play victory sound
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayBossVictorySound();
                }
                
                // Find and destroy all monsters in Floor3
                GameObject floor3 = GameObject.Find("Floor3");
                if (floor3 != null)
                {
                    // Iterate through all children of Floor3
                    foreach (Transform child in floor3.transform)
                    {
                        // Check if the child or any of its children have the Monster tag
                        if (child.CompareTag("Monster") || child.GetComponentInChildren<Transform>().CompareTag("Monster"))
                        {
                            Destroy(child.gameObject);
                        }
                    }
                }
                
                Destroy(transform.parent.gameObject);
            }
            else
            {
                needsExit = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2"))
        {
            needsExit = false;
        }
    }
}

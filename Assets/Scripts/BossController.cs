using UnityEngine;

public class BossController : MonoBehaviour
{
    private int hitsRequired = 2;
    private int currentHits = 0;
    private bool canBeHit = true;
    private bool needsExit = false;

    private void LogState(string trigger)
    {
        Debug.Log($"[{trigger}] State: " +
            $"\nHits Required: {hitsRequired}" +
            $"\nCurrent Hits: {currentHits}" +
            $"\nCan Be Hit: {canBeHit}" +
            $"\nNeeds Exit: {needsExit}" +
            $"\n------------------------");
    }

    private void Start()
    {
        LogState("Initial State");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] Collided with object tagged: {other.tag}");
        
        if (canBeHit && !needsExit && (other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2")))
        {
            Debug.Log("[Trigger Enter] Hit conditions met!");
            currentHits++;
            Debug.Log($"[Trigger Enter] Boss hit! Hits remaining: {hitsRequired - currentHits}");

            if (currentHits >= hitsRequired)
            {
                Debug.Log("[Trigger Enter] Required hits reached, destroying boss!");
                LogState("Before Destruction");
                
                // Show victory text before destroying the boss
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowVictoryMessage();
                }
                else
                {
                    Debug.LogWarning("UIManager instance not found!");
                }

                // Play victory sound
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayBossVictorySound();
                }
                else
                {
                    Debug.LogWarning("AudioManager instance not found!");
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
                            Debug.Log($"Destroying monster: {child.name}");
                            Destroy(child.gameObject);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Floor3 not found!");
                }
                
                Destroy(transform.parent.gameObject);
            }
            else
            {
                needsExit = true;
                Debug.Log("[Trigger Enter] First hit registered, waiting for exit");
                LogState("After Hit");
            }
        }
        else
        {
            Debug.Log($"[Trigger Enter] Hit conditions NOT met:" +
                $"\nCan Be Hit: {canBeHit}" +
                $"\nNeeds Exit: {needsExit}" +
                $"\nValid Tag: {other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2")}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[Trigger Exit] Object exiting trigger: {other.tag}");
        
        if (other.CompareTag("Controller") || other.CompareTag("Weapon1") || other.CompareTag("Weapon2"))
        {
            Debug.Log("[Trigger Exit] Valid object exited, resetting needsExit");
            needsExit = false;
            LogState("After Exit");
        }
        else
        {
            Debug.Log("[Trigger Exit] Non-valid object exited, no state change");
        }
    }

    private void OnDestroy()
    {
        Debug.Log("[Destroy] Boss object being destroyed");
        LogState("Final State");
    }
}

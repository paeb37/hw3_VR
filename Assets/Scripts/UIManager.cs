using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI playerDiedText; // New field for player died text
    /// <summary>
    ///  [SerializeField] private float distanceFromPlayer = 2f; // Distance in meters from the player
    /// </summary>


    // this is so other methods can access the UIManager instance
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Hide the victory text initially
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }
        
        // Hide the kill text initially
        if (killText != null)
        {
            killText.gameObject.SetActive(false);
        }
        
        // Hide the player died text initially
        if (playerDiedText != null)
        {
            playerDiedText.gameObject.SetActive(false);
        }
    }

    public void ShowVictoryMessage()
    {
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(true);
        }
    }

    public void ShowKillText()
    {
        if (killText != null)
        {
            killText.gameObject.SetActive(true);
            // Hide the text after 2 seconds
            Invoke(nameof(HideKillText), 2f);
        }
    }

    private void HideKillText()
    {
        if (killText != null)
        {
            killText.gameObject.SetActive(false);
        }
    }
    
    public void ShowPlayerDiedText()
    {
        if (playerDiedText != null)
        {
            playerDiedText.gameObject.SetActive(true);
            // Hide the text after 3 seconds
            Invoke(nameof(HidePlayerDiedText), 3f);
        }
    }
    
    private void HidePlayerDiedText()
    {
        if (playerDiedText != null)
        {
            playerDiedText.gameObject.SetActive(false);
        }
    }
} 
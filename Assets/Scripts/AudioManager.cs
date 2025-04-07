using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Monster Sounds")]
    [SerializeField] private AudioSource monsterDeathAudioSource;
    // [SerializeField] private AudioSource monsterMovementAudioSource;

    [Header("Boss Sounds")]
    [SerializeField] private AudioSource bossVictoryAudioSource;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Validate audio sources
        if (monsterDeathAudioSource == null)
        {
            Debug.LogWarning("⚠️ Monster death AudioSource is not assigned in AudioManager!");
        }
        if (bossVictoryAudioSource == null)
        {
            Debug.LogWarning("⚠️ Boss victory AudioSource is not assigned in AudioManager!");
        }
    }

    public void PlayMonsterDeathSound()
    {
        if (monsterDeathAudioSource != null && monsterDeathAudioSource.clip != null)
        {
            monsterDeathAudioSource.Play();
            Debug.Log("💀 Playing monster death sound");
        }
        else
        {
            Debug.LogWarning("⚠️ Monster death AudioSource or its clip is not assigned in AudioManager!");
        }
    }

    public void PlayBossVictorySound()
    {
        if (bossVictoryAudioSource != null && bossVictoryAudioSource.clip != null)
        {
            bossVictoryAudioSource.Play();
            Debug.Log("🎉 Playing boss victory sound");
        }
        else
        {
            Debug.LogWarning("⚠️ Boss victory AudioSource or its clip is not assigned in AudioManager!");
        }
    }

    // public void PlayMonsterMovementSound()
    // {
    //     if (monsterMovementAudioSource != null && monsterMovementAudioSource.clip != null)
    //     {
    //         monsterMovementAudioSource.Play();
    //     }
    // }

    // public void StopMonsterMovementSound()
    // {
    //     if (monsterMovementAudioSource != null)
    //     {
    //         monsterMovementAudioSource.Stop();
    //     }
    // }
} 
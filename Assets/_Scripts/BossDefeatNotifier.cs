using UnityEngine;

public class BossDefeatNotifier : MonoBehaviour
{
    [Header("Victory Audio")]
    [Tooltip("The triumphant fanfare to play when the boss falls.")]
    public AudioClip victoryFanfare;

    private bool isDefeated = false;

    public void NotifyBossDefeated()
    {
        if (isDefeated) return;
        
        isDefeated = true;
        Debug.Log("[BOSS] Skeleton defeated! Searching for Victory Manager...");
        
        // --- UPDATED: Swap the Background Music via AudioManager. The old
        // GameObject.Find("SoundManager") + SendMessage only worked in MainMenu_Scene
        // (that object doesn't exist in the gameplay levels), so the fanfare never played.
        if (victoryFanfare != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(victoryFanfare);
        }
        
        // Actively search the current scene for the VictoryManager
        VictoryManager victoryManager = Object.FindAnyObjectByType<VictoryManager>();
        
        if (victoryManager != null)
        {
            Debug.Log("[BOSS] Victory Manager found! Spawning portal...");
            victoryManager.OnLevelCompleted();
        }
        else
        {
            Debug.LogError("[BOSS FATAL ERROR] No VictoryManager found in the scene! Did you delete it?");
        }
    }
}
using UnityEngine;

public class BossDefeatNotifier : MonoBehaviour
{
    private bool isDefeated = false;

    public void NotifyBossDefeated()
    {
        if (isDefeated) return;
        
        isDefeated = true;
        Debug.Log("[BOSS] Skeleton defeated! Searching for Victory Manager...");
        
        // Actively search the current scene for the VictoryManager
        VictoryManager victoryManager = Object.FindFirstObjectByType<VictoryManager>();
        
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
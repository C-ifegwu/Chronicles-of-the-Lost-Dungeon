using UnityEngine;

public class BossDefeatNotifier : MonoBehaviour
{
    private bool isDefeated = false;

    // You will call this method when the monster's health reaches 0
    public void NotifyBossDefeated()
    {
        if (isDefeated) return;
        
        isDefeated = true;
        Debug.Log("Boss defeated! Triggering Victory UI...");
        
        if (VictoryManager.Instance != null)
        {
            VictoryManager.Instance.OnLevelCompleted();
        }
        else
        {
            Debug.LogWarning("VictoryManager is missing in the scene!");
        }
    }
}
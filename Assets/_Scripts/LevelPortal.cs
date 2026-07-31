using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelPortal : MonoBehaviour
{
    [Header("Portal Settings")]
    // variable For VictoryManager 
    public string nextSceneName; 

    private void OnTriggerEnter(Collider other)
    {
        // When the King touches the portal
        if (other.CompareTag("Player"))
        {
            // Turn on the Victory Screen and pause the game
            if (GameOverlayManager.Instance != null)
            {
                GameOverlayManager.Instance.TriggerVictory();
            }
        }
    }
}
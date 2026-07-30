using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object walking into the portal is the King
        if (other.CompareTag("Player"))
        {
            if (VictoryManager.Instance != null && !string.IsNullOrEmpty(VictoryManager.Instance.nextLevelName))
            {
                Debug.Log($"[PORTAL] King entered! Transporting to {VictoryManager.Instance.nextLevelName}...");
                SceneManager.LoadScene(VictoryManager.Instance.nextLevelName);
            }
            else
            {
                Debug.LogError("[PORTAL ERROR] Cannot transition. Next Level Name is blank in the Victory Manager!");
            }
        }
    }
}
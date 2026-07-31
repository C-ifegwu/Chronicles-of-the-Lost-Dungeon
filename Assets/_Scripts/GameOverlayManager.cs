using UnityEngine;
using UnityEngine.SceneManagement; // Required to reload or change levels

public class GameOverlayManager : MonoBehaviour
{
    public static GameOverlayManager Instance { get; private set; }

    [Header("Overlay Panels")]
    public GameObject defeatPanel;
    public GameObject victoryPanel;

    private void Awake()
    {
        // Singleton pattern so the King and Bosses can easily find this script
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerDefeat()
    {
        if (defeatPanel != null) defeatPanel.SetActive(true);
        
        // Freezes the game so enemies stop attacking while you read the screen
        Time.timeScale = 0f; 
    }

    public void TriggerVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        
        // Freezes the game 
        Time.timeScale = 0f; 
    }

    // --- Button Functions ---

    public void RestartLevel()
    {
        // Unpause the game before reloading, otherwise the new level will be frozen!
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; 
        // Loads the next scene in your Unity Build Settings queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
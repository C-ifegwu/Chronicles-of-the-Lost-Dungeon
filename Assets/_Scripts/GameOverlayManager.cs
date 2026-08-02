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

        // --- UPDATED: Ask the VictoryManager where to go instead of blindly loading
        // "buildIndex + 1". This respects the level's configured next scene and
        // correctly sends the King back to the Main Menu once the game is finished.
        if (VictoryManager.Instance != null && VictoryManager.Instance.isFinalLevel)
        {
            LoadSceneByName("MainMenu_Scene");
            return;
        }

        if (VictoryManager.Instance != null && !string.IsNullOrEmpty(VictoryManager.Instance.nextLevelName))
        {
            LoadSceneByName(VictoryManager.Instance.nextLevelName);
            return;
        }

        // Fallback for any level that doesn't have a VictoryManager configured yet
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // --- NEW: Routes through the SceneTransitionManager (loading screen) when available ---
    private void LoadSceneByName(string sceneName)
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadNextLevel(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
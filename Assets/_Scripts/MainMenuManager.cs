using UnityEngine;
using UnityEngine.UI; // --- NEW: Required to interact with UI Buttons ---
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Main Menu Elements to Hide")]
    [SerializeField] private GameObject mainMenuButtonsContainer;
    [SerializeField] private GameObject gameTitle; // Drag your Game_Title here!

    // --- NEW: Level Unlock Integration ---
    [Header("Level Progression")]
    [Tooltip("Drag your Level buttons here in order (Index 0 = Level 1, Index 1 = Level 2, etc.)")]
    public Button[] levelButtons;
    // -------------------------------------

    private void Start()
    {
        // --- NEW: Check save data and unlock levels as soon as the menu loads ---
        RefreshLevelUnlocks();
    }

    // --- NEW: The logic that reads your JSON and locks/unlocks buttons ---
    private void RefreshLevelUnlocks()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.currentData != null)
        {
            int maxUnlocked = SaveManager.Instance.currentData.highestUnlockedLevel;

            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (levelButtons[i] != null)
                {
                    // Array index 0 represents Level 1, index 1 is Level 2, etc.
                    // If maxUnlocked is 2, buttons at index 0 and 1 become interactable.
                    levelButtons[i].interactable = i < maxUnlocked;
                }
            }
        }
    }
    // ----------------------------------------------------------------------

    public void AwakenGame()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadNextLevel("Level1_TrainingPit"); 
        }
        else
        {
            SceneManager.LoadScene("Level1_TrainingPit"); 
        }
    }

    public void OpenChronicles()
    {
        // Refresh unlocks right before the panel opens just in case data changed
        RefreshLevelUnlocks(); 

        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
        if (mainMenuButtonsContainer != null) mainMenuButtonsContainer.SetActive(false);
        if (gameTitle != null) gameTitle.SetActive(false); // Hide title
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainMenuButtonsContainer != null) mainMenuButtonsContainer.SetActive(false);
        if (gameTitle != null) gameTitle.SetActive(false); // Hide title
    }

    public void CloseOverlay(GameObject panelToClose)
    {
        if (panelToClose != null) panelToClose.SetActive(false);
        if (mainMenuButtonsContainer != null) mainMenuButtonsContainer.SetActive(true);
        if (gameTitle != null) gameTitle.SetActive(true); // Bring back title
    }

    public void ReturnToVoid()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
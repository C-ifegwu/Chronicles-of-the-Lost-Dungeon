using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;

    public void AwakenGame()
    {
        // Call the unkillable loading screen manager to start the black screen sequence
        if (SceneTransitionManager.Instance != null)
        {
            // Make sure this name exactly matches your first level's file name!
            SceneTransitionManager.Instance.LoadNextLevel("Level1_TrainingPit"); 
        }
        else
        {
            Debug.LogError("Loading Screen Manager is missing! Loading instantly as fallback.");
            SceneManager.LoadScene("Level1_TrainingPit"); 
        }
    }

    public void OpenChronicles()
    {
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseOverlay(GameObject panelToClose)
    {
        if (panelToClose != null) panelToClose.SetActive(false);
    }

    public void ReturnToVoid()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
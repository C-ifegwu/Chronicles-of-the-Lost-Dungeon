using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;

    public void AwakenGame()
    {
        // Loads the main dungeon level directly
        SceneManager.LoadScene("Level1_Dungeon"); 
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
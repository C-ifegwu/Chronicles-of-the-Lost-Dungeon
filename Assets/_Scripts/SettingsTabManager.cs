using UnityEngine;

public class SettingsTabManager : MonoBehaviour
{
    [Header("Settings Panels")]
    public GameObject[] tabPanels; // Drag Audio_Panel, Controls_Panel, etc. here

    private void Start()
    {
        // Default to opening the first tab when the game/scene starts
        OpenTab(0);
    }

    private void OnEnable()
    {
        // Reset to the first tab every time the Settings panel is opened
        OpenTab(0);
    }

    public void OpenTab(int tabIndex)
    {
        // Loop through all panels and turn off the ones we didn't click
        for (int i = 0; i < tabPanels.Length; i++)
        {
            if (i == tabIndex)
            {
                tabPanels[i].SetActive(true); // Turn on the selected tab
            }
            else
            {
                tabPanels[i].SetActive(false); // Turn off the others
            }
        }
    }
}
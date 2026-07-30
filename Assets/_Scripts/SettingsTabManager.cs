using UnityEngine;

public class SettingsTabManager : MonoBehaviour
{
    [Header("Settings Panels")]
    public GameObject[] tabPanels; // Drag Audio_Panel, Controls_Panel, etc. here

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
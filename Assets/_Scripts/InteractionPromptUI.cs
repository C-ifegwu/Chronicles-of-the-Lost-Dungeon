using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    // Singleton pattern so any chest or door can easily find this script
    public static InteractionPromptUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject bannerObject;
    public TextMeshProUGUI promptText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // THIS IS THE FIX: Destroy only the script, not the Canvas!
            Destroy(this); 
            return;
        }
        
        // Ensure the banner is hidden when the game starts
        HidePrompt();
    }

    public void ShowPrompt(string message)
    {
        if (bannerObject != null && promptText != null)
        {
            promptText.text = message;
            bannerObject.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (bannerObject != null)
        {
            bannerObject.SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial Settings")]
    [TextArea(2, 5)]
    public string tutorialMessage = "Use WASD or Mobile Joystick to Move";
    public Text tutorialTextUI; // Reference to your UI Text component
    public float displayDuration = 4f;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
            if (tutorialTextUI != null)
            {
                tutorialTextUI.text = tutorialMessage;
                tutorialTextUI.gameObject.SetActive(true);
                CancelInvoke(nameof(HideText));
                Invoke(nameof(HideText), displayDuration);
            }
            else
            {
                Debug.Log("[TUTORIAL]: " + tutorialMessage);
            }
        }
    }

    private void HideText()
    {
        if (tutorialTextUI != null)
        {
            tutorialTextUI.gameObject.SetActive(false);
        }
    }
}
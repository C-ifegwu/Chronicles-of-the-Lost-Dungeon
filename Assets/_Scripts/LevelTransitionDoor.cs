using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionDoor : MonoBehaviour, IInteractable
{
    [Header("Transition Settings")]
    [Tooltip("The exact name of the next scene to load")]
    public string nextLevelName = "Level2_Dungeons";
    
    [Tooltip("The text that appears when the King gets close")]
    public string interactMessage = "Open Door to Level 2";

    public void Interact(PlayerController player)
    {
        Debug.Log($"[LEVEL TRANSITION] Leaving current level and loading {nextLevelName}...");
        SceneManager.LoadScene(nextLevelName);
    }

    public string GetInteractText()
    {
        return interactMessage;
    }
}
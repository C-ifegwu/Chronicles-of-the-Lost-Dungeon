using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelCard
    {
        public Button cardButton;
        public GameObject lockIcon;
        public TextMeshProUGUI statusText;
    }

    [Header("Level Cards (Assign Levels 1 to 5)")]
    public LevelCard[] levelCards;

    [Header("Scene Names (Index 0 = Level 1, Index 1 = Level 2, etc.)")]
    public string[] sceneNames = { "Level1_TrainingPit", "Level2_Dungeons", "Level3_PrisonRooftop", "Level4_Ward", "Level5_ThroneRoom" };

    private void OnEnable()
    {
        // This runs every time the Chronicles panel is opened
        RefreshLevelLocks();
    }

    public void RefreshLevelLocks()
    {
        // --- UPDATED: Read from the same JSON save data MainMenuManager and VictoryManager
        // use, instead of PlayerPrefs. Keeps unlocks consistent everywhere in the game.
        int maxUnlocked = 1;
        if (SaveManager.Instance != null && SaveManager.Instance.currentData != null)
        {
            maxUnlocked = SaveManager.Instance.currentData.highestUnlockedLevel;
        }

        for (int i = 0; i < levelCards.Length; i++)
        {
            int levelNumber = i + 1; // Array starts at 0, Levels start at 1

            if (levelNumber <= maxUnlocked)
            {
                // Unlock Level
                levelCards[i].cardButton.interactable = true;
                if (levelCards[i].lockIcon != null) levelCards[i].lockIcon.SetActive(false);
                if (levelCards[i].statusText != null)
                {
                    levelCards[i].statusText.text = "Status: UNLOCKED";
                    levelCards[i].statusText.color = Color.green;
                }
            }
            else
            {
                // Lock Level
                levelCards[i].cardButton.interactable = false;
                if (levelCards[i].lockIcon != null) levelCards[i].lockIcon.SetActive(true);
                if (levelCards[i].statusText != null)
                {
                    levelCards[i].statusText.text = "Complete Level " + (levelNumber - 1) + " to Unlock";
                    levelCards[i].statusText.color = Color.white;
                }
            }
        }
    }

    public void LoadLevel(int index)
    {
        if (index >= 0 && index < sceneNames.Length)
        {
            string targetScene = sceneNames[index];
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadNextLevel(targetScene);
            }
            else
            {
                SceneManager.LoadScene(targetScene);
            }
        }
        else
        {
            Debug.LogWarning("Invalid level index provided: " + index);
        }
    }
}
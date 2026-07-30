using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void OnEnable()
    {
        // This runs every time the Chronicles panel is opened
        RefreshLevelLocks();
    }

    public void RefreshLevelLocks()
    {
        // Fetches saved progress. If no save exists, defaults to Level 1.
        int maxUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

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
}
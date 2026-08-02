using System.Collections.Generic;

/// <summary>
/// This class acts as the blueprint for our JSON save file.
/// Everything inside this class will be converted to text and saved.
/// </summary>
[System.Serializable]
public class GameData
{
    // 1. Player Statistics
    public float savedMaxHealth = 100f;
    public float savedCurrentHealth = 100f;
    public int totalEnemiesSlain = 0;
    public int totalPotionsUsed = 0;

    // 2. Level Progression
    public int highestUnlockedLevel = 1;

    // 3. Inventory (Storing item names as a list of strings is the easiest way to save them via JSON)
    public List<string> unlockedWeapons = new List<string>();
    public List<string> inventoryItems = new List<string>();
    
    // 4. Settings (If we want to backup settings beyond PlayerPrefs)
    public bool hasCompletedTutorial = false;
    public float masterVolume = 1f;

    // 5. Unlockable Content
    public List<string> unlockedContent = new List<string>();

    // 6. Final Completion (Set once the King defeats the Level 5 throne room)
    public bool gameCompleted = false;

    // Constructor to set default values for a brand new game
    public GameData()
    {
        savedMaxHealth = 100f;
        savedCurrentHealth = 100f;
        totalEnemiesSlain = 0;
        totalPotionsUsed = 0;
        highestUnlockedLevel = 1;
        unlockedWeapons = new List<string>();
        inventoryItems = new List<string>();
        hasCompletedTutorial = false;
        masterVolume = 1f;
        unlockedContent = new List<string>();
        gameCompleted = false;
    }
}
using UnityEngine;

using TMPro; // --- NEW: Added so the script can talk to your TextMeshPro UI ---



public class VictoryManager : MonoBehaviour

{

    public static VictoryManager Instance { get; private set; }



    [Header("UI & Portal Settings")]

    public UnityEngine.UI.Image victoryCanvas; // Make sure your Victory_Panel is dragged into this slot!

    public GameObject portalPrefab;

    public Transform portalSpawnPoint;

    public string nextLevelName = "Level2_Dungeons";

    // --- NEW: Level Progression ---
    // Tells the save file exactly which level this manager belongs to, so completing it
    // always unlocks the correct next level instead of a hardcoded one.

    [Header("Level Progression")]

    [Tooltip("The number of THIS level (Level 1 = 1, Level 2 = 2, etc.)")]
    public int levelNumber = 1;

    [Tooltip("Check this ONLY on the last level. Instead of spawning a portal, it marks the game as completed.")]
    public bool isFinalLevel = false;
    // -------------------------------



    [Header("Victory Panel Statistics")]

    // --- UPDATED: Changed from 12 and 2 to 0 so we can track real dynamic numbers ---

    [SerializeField] private int enemiesSlainThisLevel = 0;

    [SerializeField] private int potionsUsedThisLevel = 0;  



    // --- NEW: Slots to drag your UI Text objects into ---

    [Header("Victory UI Text Links")]

    [SerializeField] private TMP_Text enemiesSlainTextUI;

    [SerializeField] private TMP_Text potionsUsedTextUI;

    // ----------------------------------------------------



    private void Awake()

    {

        if (Instance == null)

        {

            Instance = this;

        }

        else

        {

            Destroy(gameObject);

        }

    }



    // --- NEW: Hide the canvas the exact moment the level loads ---

    private void Start()

    {

        if (victoryCanvas != null)

        {

            victoryCanvas.gameObject.SetActive(false);

        }

    }

    // -------------------------------------------------------------



    // --- NEW: Public methods to update stats dynamically during gameplay ---

    public void AddEnemyKilled()

    {

        enemiesSlainThisLevel++;

    }



    public void AddPotionUsed()

    {

        potionsUsedThisLevel++;

    }

    // -----------------------------------------------------------------------



    // Called by the BossDefeatNotifier when the skeleton's health hits 0

    public void OnLevelCompleted()

    {

        Debug.Log("Level Objective Complete! Spawning portal now...");



        // --- NEW: Activating the Victory Canvas so the UI appears ---

        if (victoryCanvas != null)

        {

            victoryCanvas.gameObject.SetActive(true);

        }

        // ------------------------------------------------------------



        // --- Push the raw numbers to the actual screen UI ---

        if (enemiesSlainTextUI != null)

        {

            enemiesSlainTextUI.text = "Enemies Slain: " + enemiesSlainThisLevel.ToString();

        }

        if (potionsUsedTextUI != null)

        {

            potionsUsedTextUI.text = "Potions Used: " + potionsUsedThisLevel.ToString();

        }

        // --------------------------------------------------------



        // --- JSON Save Integration (Full Rubric Coverage) ---

        if (SaveManager.Instance != null)

        {

            GameData data = SaveManager.Instance.currentData;



            // 1. Level Progression
            // --- UPDATED: Generalized so ANY level unlocks the level right after it,
            // instead of always hardcoding Level 2. The final level marks the game complete.

            if (isFinalLevel)

            {

                data.gameCompleted = true;

            }

            else if (data.highestUnlockedLevel < levelNumber + 1)

            {

                data.highestUnlockedLevel = levelNumber + 1;

            }



            // 2. Player Statistics (Capturing Victory Panel metrics)

            data.totalEnemiesSlain += enemiesSlainThisLevel;

            data.totalPotionsUsed += potionsUsedThisLevel;



            // 3. Inventory & Unlockable Content

            if (!data.inventoryItems.Contains("Dungeon Key"))

            {

                data.inventoryItems.Add("Dungeon Key");

            }

            if (!data.unlockedContent.Contains("Level_2_Access"))

            {

                data.unlockedContent.Add("Level_2_Access");

            }

           

            // Trigger the JSON write to disk

            SaveManager.Instance.SaveGame();

        }

        // ----------------------------------



        // --- REST API Leaderboard Integration ---

        if (LeaderboardManager.Instance != null)

        {

            // Sends a score based on level performance to the online cloud database via REST API

            LeaderboardManager.Instance.SendScore("KingVictor", enemiesSlainThisLevel * 100);

        }

        // ---------------------------------------------



        // --- NEW: The final level has no next stage, so no portal should spawn.
        // The Victory_Panel's Continue_Button will send the King back to the Main Menu instead.

        if (!isFinalLevel)

        {

            SpawnPortal();

        }

    }



    private void SpawnPortal()

    {

        if (portalPrefab != null && portalSpawnPoint != null)

        {

            // Spawns the portal into the 3D world

            GameObject portalInstance = Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);

            LevelPortal portalScript = portalInstance.GetComponent<LevelPortal>();

           

            if (portalScript != null)

            {

                portalScript.nextSceneName = nextLevelName;

            }



            Debug.Log("Magic Portal Spawned at: " + portalSpawnPoint.position);

        }

        else

        {

            Debug.LogWarning("Portal Prefab or Spawn Point reference missing in VictoryManager!");

        }

    }

} 
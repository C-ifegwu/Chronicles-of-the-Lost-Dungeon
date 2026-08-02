using UnityEngine;
using System.IO; // Required for reading and writing files

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    [Header("Active Game Data")]
    public GameData currentData;
    
    // The hidden path on the computer where the file will be stored safely
    private string saveFilePath;

    private void Awake()
    {
        // Set up the Singleton so this manager persists across scenes
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            // Application.persistentDataPath automatically finds the correct safe folder on Windows/Mac/Android
            saveFilePath = Application.persistentDataPath + "/kings_savefile.json";
            
            // Automatically load data when the game boots up
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Converts the current data to JSON and writes it to the hard drive.
    /// </summary>
    public void SaveGame()
    {
        // 1. Convert the C# GameData object into a formatted JSON string
        string jsonText = JsonUtility.ToJson(currentData, true);
        
        // 2. Write that string to the physical file
        File.WriteAllText(saveFilePath, jsonText);
        
        Debug.Log("JSON Data successfully saved to: " + saveFilePath);
    }

    /// <summary>
    /// Reads the JSON file from the hard drive and converts it back into active game data.
    /// </summary>
    public void LoadGame()
    {
        // 1. Check if a save file actually exists (prevents errors on first playthrough)
        if (File.Exists(saveFilePath))
        {
            // 2. Read the text from the file
            string jsonText = File.ReadAllText(saveFilePath);
            
            // 3. Convert the JSON text back into our GameData object structure
            currentData = JsonUtility.FromJson<GameData>(jsonText);
            
            Debug.Log("JSON Data successfully loaded.");
        }
        else
        {
            // If no save file exists, create a brand new, clean GameData blueprint
            Debug.Log("No save file found. Generating new GameData.");
            currentData = new GameData();
        }
    }
}
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance { get; private set; }

    [Header("UI & Portal Settings")]
    public GameObject victoryCanvas; // You can leave this blank in the Inspector!
    public GameObject portalPrefab;
    public Transform portalSpawnPoint;
    public string nextLevelName = "Level2_Dungeons";

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

    // Called by the BossDefeatNotifier when the skeleton's health hits 0
    public void OnLevelCompleted()
    {
        Debug.Log("Level Objective Complete! Spawning portal now...");
        SpawnPortal();
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
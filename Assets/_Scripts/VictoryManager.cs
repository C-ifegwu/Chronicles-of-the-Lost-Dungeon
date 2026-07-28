using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance { get; private set; }

    [Header("UI & Portal Settings")]
    public GameObject victoryCanvas;
    public GameObject portalPrefab;
    public Transform portalSpawnPoint;
    public string nextLevelName;

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

    private void Start()
    {
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }
    }

    // Call this method when the level boss/challenge is defeated
    public void OnLevelCompleted()
    {
        Debug.Log("Level Objective Complete!");
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(true);
        }
    }

    // Assign this method to the "Proceed" Button OnClick() listener in the Inspector
    public void OnProceedButtonClicked()
    {
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }

        SpawnPortal();
    }

    private void SpawnPortal()
    {
        if (portalPrefab != null && portalSpawnPoint != null)
        {
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
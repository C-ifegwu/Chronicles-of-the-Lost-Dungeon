using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Mobile Controls")]
    [SerializeField] private GameObject mobileControlsGroup; // The on-screen joysticks/buttons

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ConfigurePlatformUI();
    }

    private void OnEnable()
    {
        // Subscribe to events (Observer Pattern)
        GameEvents.OnPlayerHealthChanged += UpdateHealthUI;
        GameEvents.OnPlayerDied += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        GameEvents.OnPlayerHealthChanged -= UpdateHealthUI;
        GameEvents.OnPlayerDied -= ShowGameOverScreen;
    }

    private void ConfigurePlatformUI()
    {
        #if UNITY_ANDROID || UNITY_IOS
            if(mobileControlsGroup != null) mobileControlsGroup.SetActive(true);
        #elif UNITY_STANDALONE_WIN || UNITY_WEBGL || UNITY_EDITOR
            if(mobileControlsGroup != null) mobileControlsGroup.SetActive(false);
        #endif
    }

    // --- Event Listener Methods ---

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // We will link this to your visual health bar later
        Debug.Log($"UI Updated: Health is now {currentHealth}/{maxHealth}");
    }

    private void ShowGameOverScreen()
    {
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    // --- Button Methods ---
    
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        hudPanel.SetActive(true);
        // Logic to load Level 1 will go here
    }
}
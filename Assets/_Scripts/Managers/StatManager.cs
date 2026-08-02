using UnityEngine;
using UnityEngine.UI; // Required to communicate with your HUD Sliders

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    [Header("King's Dynamic Stats")]
    public float currentMaxHealth = 100f;
    public float currentHealth;
    public float currentAttackDamage = 20f;
    public float maxStamina = 100f;
    public float currentStamina;

    [Header("HUD Connections")]
    public Slider healthBarSlider;
    public Slider staminaBarSlider;

    private void Awake()
    {
        // Singleton pattern to preserve upgraded stats across all levels
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Fill up health and stamina when the level starts
        currentHealth = currentMaxHealth;
        currentStamina = maxStamina;
        UpdateUI();
    }

    // --- NEW: The Missing Engine! This forces the UI to continuously match the King's math ---
    private void Update()
    {
        UpdateUI();
    }

    // --- Combat Logic ---

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;
        
        UpdateUI();
        
        // --- UPDATED: PlayerController.Die() is now the single source of truth for defeat
        // (it mirrors this exact health value into StatManager every frame). Triggering
        // the Defeat_Panel from here too would fire it twice, so this now just tracks the
        // number for the HUD/upgrades and lets PlayerController own the actual death event.
        if (currentHealth == 0)
        {
            Debug.Log("The King has fallen!");
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > currentMaxHealth) currentHealth = currentMaxHealth;
        
        UpdateUI();
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0;
        
        UpdateUI();
    }

    // --- UI Logic ---

    private void UpdateUI()
    {
        // Automatically scales the slider to match the exact max health/stamina
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = currentMaxHealth;
            healthBarSlider.value = currentHealth;
        }

        if (staminaBarSlider != null)
        {
            staminaBarSlider.maxValue = maxStamina;
            staminaBarSlider.value = currentStamina;
        }
    }

    // --- Permanent Upgrades ---

    public void IncreaseMaxHealth(float amount)
    {
        currentMaxHealth += amount;
        currentHealth += amount; // Heals the King for the new extra amount
        UpdateUI();
        Debug.Log($"[STAT UPGRADE] Max Health increased by {amount}! New Max Health: {currentMaxHealth}");
    }

    public void IncreaseAttackDamage(float amount)
    {
        currentAttackDamage += amount;
        Debug.Log($"[STAT UPGRADE] Attack Damage increased by {amount}! New Attack Damage: {currentAttackDamage}");
    }
}
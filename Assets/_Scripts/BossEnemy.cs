using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossEnemy : MonoBehaviour
{
    [Header("Boss Identity")]
    public string bossName = "Training Pit Skeleton";

    [Header("UI References (Leave these empty!)")]
    [Tooltip("The script will automatically find the UI in the scene at runtime.")]
    public GameObject bossUIContainer;
    public Slider bossHealthBar;
    public TextMeshProUGUI bossNameText;

    private void Awake()
    {
        // Auto-wire the UI so the Prefab doesn't need to save scene references!
        if (bossUIContainer == null)
        {
            // Searches the current scene for the container we built earlier
            GameObject containerObj = GameObject.Find("Boss_UI_Container");
            
            if (containerObj != null)
            {
                bossUIContainer = containerObj;
                
                // Grabs the Slider and Text hidden inside that container
                bossHealthBar = containerObj.GetComponentInChildren<Slider>(true);
                bossNameText = containerObj.GetComponentInChildren<TextMeshProUGUI>(true);
            }
            else
            {
                Debug.LogWarning($"[Boss UI] Could not find 'Boss_UI_Container' in this scene. Did you forget to add the HUD_Canvas prefab?");
            }
        }
    }

    // Called once when the fight starts (e.g. by your EnemyController Start method)
    public void ActivateBossUI(float maxHealth)
    {
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(true);
            
            if (bossNameText != null) 
                bossNameText.text = bossName;

            if (bossHealthBar != null)
            {
                bossHealthBar.maxValue = maxHealth;
                bossHealthBar.value = maxHealth;
            }
        }
    }

    // Called by your EnemyController every time the boss takes damage
    public void UpdateHealthBar(float currentHealth)
    {
        if (bossHealthBar != null)
        {
            bossHealthBar.value = currentHealth;
        }
    }

    // Called by your EnemyController when health reaches 0
    public void HideBossUI()
    {
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossEnemy : MonoBehaviour
{
    [Header("Boss Identity")]
    public string bossName = "Skeleton Soldier";
    public float maxHealth = 200f;
    private float currentHealth;

    [Header("UI References")]
    public GameObject bossUIContainer;
    public Slider bossHealthBar;
    public TextMeshProUGUI bossNameText;

    private bool isFightActive = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this when the player enters the boss arena or when the boss spots the player
    public void ActivateBossFight()
    {
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(true);
            
            if (bossNameText != null) 
                bossNameText.text = bossName;

            if (bossHealthBar != null)
            {
                bossHealthBar.maxValue = maxHealth;
                bossHealthBar.value = currentHealth;
            }

            isFightActive = true;
        }
    }

    public void TakeBossDamage(float amount)
    {
        if (!isFightActive) ActivateBossFight();

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        // Update the central UI bar
        if (bossHealthBar != null)
        {
            bossHealthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            OnBossDefeated();
        }
    }

    private void OnBossDefeated()
    {
        isFightActive = false;
        
        // Turn off the boss UI when defeated
        if (bossUIContainer != null)
        {
            bossUIContainer.SetActive(false);
        }

        Debug.Log($"{bossName} has been slain!");
        // Trigger Victory or drop key here
    }
}
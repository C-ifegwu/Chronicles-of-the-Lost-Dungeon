using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    [Header("King's Dynamic Stats")]
    public float currentMaxHealth = 100f;
    public float currentAttackDamage = 20f;

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

    /// <summary>
    /// Permanently increases the King's maximum health pool.
    /// </summary>
    public void IncreaseMaxHealth(float amount)
    {
        currentMaxHealth += amount;
        Debug.Log($"[STAT UPGRADE] Max Health increased by {amount}! New Max Health: {currentMaxHealth}");
    }

    /// <summary>
    /// Permanently increases the King's base melee and special attack damage.
    /// </summary>
    public void IncreaseAttackDamage(float amount)
    {
        currentAttackDamage += amount;
        Debug.Log($"[STAT UPGRADE] Attack Damage increased by {amount}! New Attack Damage: {currentAttackDamage}");
    }
}
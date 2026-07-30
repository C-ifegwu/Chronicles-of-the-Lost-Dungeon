using UnityEngine;

public class ConsumablePickup : MonoBehaviour, IInteractable
{
    [Header("Consumable Settings")]
    public int healthRestoreAmount = 25;
    public float staminaRestoreAmount = 25f;
    public string interactText = "Pick up Rejuvenation Potion";

    public void Interact(PlayerController player)
    {
        // Apply both effects at the same time
        player.Heal(healthRestoreAmount);
        player.RestoreStamina(staminaRestoreAmount);
        
        Debug.Log($"Restored {healthRestoreAmount} Health and {staminaRestoreAmount} Stamina!");

        // Destroy the bottle after drinking
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
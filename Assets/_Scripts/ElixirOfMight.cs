using UnityEngine;

public class ElixirOfMight : MonoBehaviour, IInteractable
{
    [Header("Potion Settings")]
    public float damageBoostAmount = 15f;
    public string interactText = "Drink Elixir of Might (+Damage)";
    
    [Header("VFX & Audio")]
    public GameObject consumeVFX;

    public void Interact(PlayerController player)
    {
        Debug.Log("[POTION] Drinking Elixir of Might...");

        // Find the StatManager and boost the damage
        StatManager stats = Object.FindAnyObjectByType<StatManager>();
        if (stats != null)
        {
            stats.IncreaseAttackDamage(damageBoostAmount);
        }
        else
        {
            Debug.LogWarning("StatManager not found in scene!");
        }

        // Spawn visual effects if assigned
        if (consumeVFX != null)
        {
            Instantiate(consumeVFX, transform.position, Quaternion.identity);
        }

        // Destroy the potion after drinking
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
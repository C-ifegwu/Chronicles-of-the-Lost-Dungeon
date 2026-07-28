using UnityEngine;

public class Potion : MonoBehaviour, IInteractable
{
    public enum PotionType { Vitality, Might }
    
    [Header("Potion Settings")]
    [Tooltip("Vitality increases Health. Might increases Damage.")]
    public PotionType type;
    public float boostAmount = 50f;
    public string interactText = "Drink Elixir";

    [Header("Visuals")]
    [Tooltip("Drag a Hovl Studio VFX prefab here")]
    public GameObject consumeVFX; 

    public void Interact(PlayerController player)
    {
        // Apply the correct stat boost based on the type of potion
        if (type == PotionType.Vitality)
        {
            if (StatManager.Instance != null)
            {
                StatManager.Instance.IncreaseMaxHealth(boostAmount);
            }
            else
            {
                Debug.LogWarning("StatManager is missing! Cannot increase Health.");
            }
        }
        else if (type == PotionType.Might)
        {
            if (StatManager.Instance != null)
            {
                StatManager.Instance.IncreaseAttackDamage(boostAmount);
            }
            else
            {
                Debug.LogWarning("StatManager is missing! Cannot increase Damage.");
            }
        }

        // Spawn the magical aura effect directly on the King
        if (consumeVFX != null)
        {
            Instantiate(consumeVFX, player.transform.position, Quaternion.identity, player.transform);
        }

        // Destroy the potion bottle from the scene
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
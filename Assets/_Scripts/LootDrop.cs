using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Header("Loot Settings")]
    [Tooltip("The prefab to drop (e.g., Rejuvenation Potion)")]
    public GameObject itemPrefab;
    
    [Tooltip("Percentage chance to drop the item (0 to 100)")]
    [Range(0f, 100f)] 
    public float dropChance = 30f;

    [Tooltip("How high off the ground the item should spawn")]
    public float dropHeightOffset = 1.0f;

    public void TryDropLoot()
    {
        if (itemPrefab == null) return;

        // Generate a random number between 0 and 100
        float roll = Random.Range(0f, 100f);

        // If the roll is less than or equal to the drop chance, spawn the item
        if (roll <= dropChance)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, dropHeightOffset, 0);
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"[LOOT] {gameObject.name} dropped {itemPrefab.name}!");
        }
    }
}
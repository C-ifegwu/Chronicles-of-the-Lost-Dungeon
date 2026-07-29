using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("Loot Drop Settings")]
    [Tooltip("Drag the 3D Rust Key prefab here")]
    public GameObject itemToDrop;
    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0);

    private bool hasDropped = false;

    /// <summary>
    /// Spawns the assigned key/loot prefab at the enemy's position upon defeat.
    /// </summary>
    public void DropItem()
    {
        if (hasDropped) return;
        hasDropped = true;

        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position + spawnOffset, Quaternion.identity);
            Debug.Log($"[LOOT DROP] Dropped {itemToDrop.name} at {transform.position}");
        }
        else
        {
            Debug.LogWarning("[LOOT DROP] No item assigned to drop!");
        }
    }
}
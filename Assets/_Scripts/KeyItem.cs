using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public string keyID = "WardenKey";
    public string interactText = "Pick up Warden's Key";
    public int keyValue = 1000;
    
    [Header("VFX & Audio")]
    public GameObject pickupVFX;

    public void Interact(PlayerController player)
    {
        Debug.Log($"[KEY PICKUP] Collected key: {keyID}");

        // --- UPDATED: Reconnected to the real InventorySorter (the old QuickSortInventory
        // reference no longer exists, which is why this call was disabled before).
        if (InventorySorter.Instance != null)
        {
            InventorySorter.Instance.AddItem(keyID, keyValue);
        }

        // Spawn pickup visual effects if assigned
        if (pickupVFX != null)
        {
            Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }

        // Destroy the key object from the world
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
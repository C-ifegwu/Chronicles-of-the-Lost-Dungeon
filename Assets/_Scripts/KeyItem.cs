using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public string keyID = "WardenKey";
    public string interactText = "Pick up Warden's Key";
    
    [Header("VFX & Audio")]
    public GameObject pickupVFX;

    public void Interact(PlayerController player)
    {
        Debug.Log($"[KEY PICKUP] Collected key: {keyID}");

        // TEMPORARILY COMMENTED OUT to clear the CS0246 error.
        // We will reconnect this once the Inventory manager is finalized!
        /*
        QuickSortInventory inventory = FindAnyObjectByType<QuickSortInventory>();
        if (inventory != null)
        {
            inventory.AddItem(keyID);
        }
        */

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
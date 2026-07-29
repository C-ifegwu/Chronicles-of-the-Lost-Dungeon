using UnityEngine;
using System.Collections;

public class CellDoor : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public string requiredKeyID = "WardenKey";
    public string lockedText = "Cell locked. Find the Warden's Key.";
    public string unlockedText = "Open Cell Door";
    
    [Header("Code-Based Animation")]
    [Tooltip("How far the door moves when opened. Default moves it down into the floor.")]
    public Vector3 slideOffset = new Vector3(0, -3.5f, 0); 
    public float slideDuration = 1.5f;

    private bool isUnlocked = false;
    private bool isOpening = false;

    public void Interact(PlayerController player)
    {
        if (isUnlocked || isOpening) return;

        // TEMPORARILY COMMENTED OUT to avoid CS0246 errors.
        /*
        QuickSortInventory inventory = Object.FindAnyObjectByType<QuickSortInventory>();
        if (inventory == null || !inventory.HasItem(requiredKeyID))
        {
            Debug.Log("You need the Warden's Key to open this!");
            return;
        }
        */

        UnlockAndOpen();
    }

    private void UnlockAndOpen()
    {
        isUnlocked = true;
        isOpening = true;
        Debug.Log($"[CELL DOOR] Unlocked with {requiredKeyID}");

        // Disable the collider so the King can walk through into the boss room
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        // Start sliding the door smoothly
        StartCoroutine(SlideDoorRoutine());
    }

    private IEnumerator SlideDoorRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + slideOffset;
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            // Lerp smoothly interpolates the position over time
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure it snaps exactly to the final position when done
        transform.position = endPos;
        isOpening = false;
    }

    public string GetInteractText()
    {
        return isUnlocked ? "" : lockedText;
    }
}
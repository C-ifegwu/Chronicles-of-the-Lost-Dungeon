using UnityEngine;
using System.Collections.Generic;

public class GauntletManager : MonoBehaviour
{
    [Header("Gauntlet Enemies")]
    [Tooltip("Drag all the enemies for this wave into this list.")]
    public List<GameObject> waveEnemies;

    private bool gauntletTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Prevent triggering multiple times
        if (gauntletTriggered) return;

        // Check if the object crossing the trigger is the King
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            TriggerAmbush();
        }
    }

    // --- CHANGED: Now public so NUnit Tests can access it ---
    public void TriggerAmbush()
    {
        gauntletTriggered = true;
        Debug.Log("[GAUNTLET] The ambush has been triggered!");

        // Loop through the list and wake up every enemy
        foreach (GameObject enemy in waveEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
    }
}
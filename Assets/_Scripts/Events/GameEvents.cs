using System;
using UnityEngine;

public static class GameEvents 
{
    // Health and Combat Events
    public static Action<int, int> OnPlayerHealthChanged; // Current Health, Max Health
    public static Action OnPlayerDied;
    public static Action<GameObject> OnEnemyDefeated;

    // Level and Progression Events
    public static Action<int> OnLevelCompleted; // Passes the level ID
    public static Action OnGameSaved;
    
    // UI Events
    public static Action<bool> OnPauseToggled; // True for paused, False for resumed
}
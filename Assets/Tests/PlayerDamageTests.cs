using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerDamageTests
{
    [Test]
    public void Player_TakeLethalDamage_DisablesController()
    {
        // 1. Arrange: Set up a dummy player object with required components
        GameObject playerObject = new GameObject("TestKing");
        PlayerController player = playerObject.AddComponent<PlayerController>();
        playerObject.AddComponent<CharacterController>();
        playerObject.AddComponent<PlayerAnimator>();
        
        // 2. Act: Deal exactly 100 damage (the default max health)
        player.TakeDamage(100);

        // 3. Assert: Prove that the death logic ran by checking if the script disabled itself
        Assert.IsFalse(player.enabled, "The PlayerController script should disable itself upon dying.");
        
        // Cleanup
        GameObject.DestroyImmediate(playerObject);
    }
}
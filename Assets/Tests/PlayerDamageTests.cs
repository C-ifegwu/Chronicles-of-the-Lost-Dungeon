using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerDamageTests
{
    [UnityTest]
    public IEnumerator Player_TakeLethalDamage_DisablesController()
    {
        // 1. Arrange: Set up a dummy player object with required components
        GameObject playerObject = new GameObject("TestKing");
        PlayerController player = playerObject.AddComponent<PlayerController>();
        playerObject.AddComponent<CharacterController>();
        playerObject.AddComponent<PlayerAnimator>();
        
        // Wait exactly one frame to allow Unity to run Start() and link the components together
        yield return null;

        // 2. Act: Deal exactly 100 damage (the default max health)
        player.TakeDamage(100);

        // 3. Assert: Prove that the death logic ran by checking if the script disabled itself
        Assert.IsFalse(player.enabled, "The PlayerController script should disable itself upon dying.");
        
        // Cleanup
        Object.Destroy(playerObject);
    }
}
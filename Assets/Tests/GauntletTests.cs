using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GauntletTests
{
    private GameObject triggerObject;
    private GauntletManager gauntletManager;
    private GameObject testEnemy;

    [SetUp]
    public void Setup()
    {
        // Arrange: Set up the trigger and a sleeping enemy
        triggerObject = new GameObject("Test_GauntletTrigger");
        gauntletManager = triggerObject.AddComponent<GauntletManager>();

        testEnemy = new GameObject("Test_SleepingEnemy");
        testEnemy.SetActive(false); // Enemy starts deactivated

        // Add the sleeping enemy to the manager's wave list
        gauntletManager.waveEnemies = new List<GameObject> { testEnemy };
    }

    [Test]
    public void GauntletManager_WakesUpEnemies_Correctly()
    {
        // Act: Simulate the player walking into the tripwire
        gauntletManager.TriggerAmbush();

        // Assert: Verify the enemy was forced to wake up
        Assert.IsTrue(testEnemy.activeSelf, "The enemy failed to activate when the gauntlet was triggered!");
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up memory after the test
        Object.DestroyImmediate(triggerObject);
        Object.DestroyImmediate(testEnemy);
    }
}
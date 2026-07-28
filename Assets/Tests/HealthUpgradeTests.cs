using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthUpgradeTests
{
    private GameObject managerObject;
    private StatManager statManager;

    [SetUp]
    public void Setup()
    {
        // Arrange: Create a fresh StatManager object before every test runs
        managerObject = new GameObject("Test_StatManager");
        statManager = managerObject.AddComponent<StatManager>();
        
        // Set a baseline health
        statManager.currentMaxHealth = 100f;
    }

    [Test]
    public void ElixirOfVitality_IncreasesMaxHealth_Correctly()
    {
        // Arrange
        float initialHealth = statManager.currentMaxHealth;
        float boostAmount = 50f;
        float expectedHealth = 150f;

        // Act: Simulate drinking the Elixir of Vitality
        statManager.IncreaseMaxHealth(boostAmount);

        // Assert: Verify the logic holds up
        Assert.AreEqual(expectedHealth, statManager.currentMaxHealth, "Max health did not scale to the mathematically correct value.");
        Assert.Greater(statManager.currentMaxHealth, initialHealth, "New max health failed to be strictly greater than the initial health.");
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the temporary object to prevent memory leaks in the test runner
        Object.DestroyImmediate(managerObject);
    }
}
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DamageUpgradeTests
{
    private GameObject managerObject;
    private StatManager statManager;

    [SetUp]
    public void Setup()
    {
        // Arrange: Create a fresh StatManager object before every test runs
        managerObject = new GameObject("Test_StatManager");
        statManager = managerObject.AddComponent<StatManager>();
        
        // Set a baseline attack damage
        statManager.currentAttackDamage = 20f;
    }

    [Test]
    public void ElixirOfMight_IncreasesAttackDamage_Correctly()
    {
        // Arrange
        float initialDamage = statManager.currentAttackDamage;
        float boostAmount = 15f;
        float expectedDamage = 35f; // 20 + 15

        // Act: Simulate drinking the Elixir of Might
        statManager.IncreaseAttackDamage(boostAmount);

        // Assert: Verify the logic calculates correctly
        Assert.AreEqual(expectedDamage, statManager.currentAttackDamage, "Attack damage did not scale to the mathematically correct value.");
        Assert.Greater(statManager.currentAttackDamage, initialDamage, "New attack damage failed to be strictly greater than the initial damage.");
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the temporary object
        Object.DestroyImmediate(managerObject);
    }
}
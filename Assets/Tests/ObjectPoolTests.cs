using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectPoolTests
{
    private GameObject poolGameObject;
    private ObjectPool objectPool;
    private GameObject testPrefab;

    [SetUp]
    public void Setup()
    {
        // 1. Create the GameObject but immediately turn it off
        poolGameObject = new GameObject();
        poolGameObject.SetActive(false);
        
        // 2. Add the component. Because it is inactive, Awake() is NOT called yet!
        objectPool = poolGameObject.AddComponent<ObjectPool>();
        
        testPrefab = new GameObject("TestArrow");
        
        // 3. Inject the test prefab safely
        var prefabField = typeof(ObjectPool).GetField("prefabToPool", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        prefabField.SetValue(objectPool, testPrefab);
        
        // 4. Activate the GameObject. Unity now fires Awake() safely with our prefab loaded!
        poolGameObject.SetActive(true);
    }

    [UnityTest]
    public IEnumerator ObjectPool_RetrievesAndReturnsObjectSuccessfully()
    {
        // Wait one frame to ensure initialization is complete
        yield return null;

        GameObject retrievedObj = objectPool.GetPooledObject();
        
        Assert.IsNotNull(retrievedObj);
        Assert.IsTrue(retrievedObj.activeInHierarchy);

        objectPool.ReturnToPool(retrievedObj);

        Assert.IsFalse(retrievedObj.activeInHierarchy);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(poolGameObject);
        Object.Destroy(testPrefab);
    }
}
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string itemName;
    public int itemValue; // Higher value items get sorted to the top

    public LootItem(string name, int value)
    {
        itemName = name;
        itemValue = value;
    }
}

public class InventorySorter : MonoBehaviour
{
    public static InventorySorter Instance { get; private set; }

    [Header("King's Inventory")]
    public List<LootItem> kingsLoot = new List<LootItem>();

    private void Awake()
    {
        // Singleton setup so the inventory persists across all 5 levels
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Adding dummy items to test the sorting algorithm
        kingsLoot.Add(new LootItem("Basic Rations", 10));
        kingsLoot.Add(new LootItem("Elixir of Vitality", 500));
        kingsLoot.Add(new LootItem("Warden's Key", 1000));
        kingsLoot.Add(new LootItem("Rusty Sword", 15));
        
        SortInventory();
    }

    public void AddItem(string name, int value)
    {
        kingsLoot.Add(new LootItem(name, value));
        SortInventory();
    }

    public void SortInventory()
    {
        Debug.Log("Sorting Inventory with Quick Sort...");
        if (kingsLoot.Count > 0)
        {
            QuickSort(kingsLoot, 0, kingsLoot.Count - 1);
        }

        // Print to console to verify the sort worked
        foreach (LootItem item in kingsLoot)
        {
            Debug.Log(item.itemName + " - Value: " + item.itemValue);
        }
    }

    // --- ALGORITHM 3: QUICK SORT ---
    private void QuickSort(List<LootItem> array, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(array, low, high);
            QuickSort(array, low, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, high);
        }
    }

    private int Partition(List<LootItem> array, int low, int high)
    {
        int pivotValue = array[high].itemValue;
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            // Sort in descending order (Highest value first)
            if (array[j].itemValue >= pivotValue)
            {
                i++;
                Swap(array, i, j);
            }
        }
        Swap(array, i + 1, high);
        return i + 1;
    }

    private void Swap(List<LootItem> array, int indexA, int indexB)
    {
        LootItem temp = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = temp;
    }
}
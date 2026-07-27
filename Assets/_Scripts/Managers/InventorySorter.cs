using System.Collections.Generic;
using UnityEngine;

// A simple class to represent an item the King collects
[System.Serializable]
public class LootItem
{
    public string itemName;
    public int itemValue;

    public LootItem(string name, int value)
    {
        itemName = name;
        itemValue = value;
    }
}

public class InventorySorter : MonoBehaviour
{
    [Header("Test Inventory")]
    public List<LootItem> kingsLoot = new List<LootItem>();

    private void Start()
    {
        // Add some dummy items for testing
        kingsLoot.Add(new LootItem("Rusty Sword", 15));
        kingsLoot.Add(new LootItem("Gold Coin", 50));
        kingsLoot.Add(new LootItem("Health Potion", 25));
        kingsLoot.Add(new LootItem("Diamond Ring", 500));
        kingsLoot.Add(new LootItem("Leather Boots", 10));
        SortInventory();
    }

    // You can call this from a UI button to sort the inventory instantly!
    public void SortInventory()
    {
        Debug.Log("Sorting Inventory from Highest to Lowest Value...");
        QuickSort(kingsLoot, 0, kingsLoot.Count - 1);
        
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
            // Find the partition index
            int pivotIndex = Partition(array, low, high);

            // Recursively sort elements before and after the partition
            QuickSort(array, low, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, high);
        }
    }

    private int Partition(List<LootItem> array, int low, int high)
    {
        // Choose the last element's value as the pivot
        int pivotValue = array[high].itemValue;
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            // We want highest value first, so we check if it is GREATER than the pivot
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
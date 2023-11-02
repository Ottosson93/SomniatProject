using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ItemDropSystem : MonoBehaviour
{
    public ItemPrefabMapper itemPrefabMapper;

    private Dictionary<int, List<ItemDropInfo>> itemDropTable;

    private List<ItemDropInfo> itemDrops = new List<ItemDropInfo>();


    [System.Serializable]
    public class ItemDropInfo
    {
        public int itemID;
        public string itemName;
        public float chestDropRate;
        public float enemyDropRate;
    }

    void Start()
    {
        LoadItemDropRatesFromCSV("Prefabs/Items/ItemDropRates.csv");
    }

    void LoadItemDropRatesFromCSV(string csvFileName)
    {
        itemDrops.Clear();

        string path = Path.Combine(Application.dataPath, csvFileName);

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    int itemID = int.Parse(values[0]);
                    string itemName = values[1];
                    float chestDropRate = float.Parse(values[2]);
                    float enemyDropRate = float.Parse(values[3]);

                    ItemDropInfo itemDropInfo = new ItemDropInfo
                    {
                        itemID = itemID,
                        itemName = itemName,
                        chestDropRate = chestDropRate,
                        enemyDropRate = enemyDropRate
                    };

                    itemDrops.Add(itemDropInfo);
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not found: " + path);
        }
    }

    public void DropItem(Vector3 dropLocation, int itemID)
    {
        List<ItemDropInfo> itemDrops;

        if (itemDropTable.TryGetValue(itemID, out itemDrops))
        {
            string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: false);

            if (itemToDrop != null)
            {
                GameObject itemPrefab = itemPrefabMapper.GetItemPrefab(itemToDrop);

                if (itemPrefab != null)
                {
                    Instantiate(itemPrefab, dropLocation, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Item prefab not found for: " + itemToDrop);
                }
            }
        }
        else
        {
            Debug.LogWarning("Item ID not found in item drop mapping: " + itemID);
        }
    }

    string DetermineItemToDrop(List<ItemDropInfo> itemDrops, bool isChestDrop)
    {
        float totalWeight = 0f;
        float randomValue = Random.Range(0f, 1f);

        List<ItemDropInfo> validDrops = itemDrops;

        if (isChestDrop)
        {
            validDrops = itemDrops.Where(drop => drop.chestDropRate > 0).ToList();
        }
        else
        {
            validDrops = itemDrops.Where(drop => drop.enemyDropRate > 0).ToList();
        }

        foreach (var itemDrop in validDrops)
        {
            float dropRate = isChestDrop ? itemDrop.chestDropRate : itemDrop.enemyDropRate;

            totalWeight += dropRate;

            if (randomValue < totalWeight)
            {
                return itemDrop.itemName;
            }
        }

        return null;
    }

    public void HandleEnemyDeath(Vector3 enemyPosition)
    {
        string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: false);

        HandleDroppedItem(itemToDrop, enemyPosition);
    }

    public void HandleChestOpen(Vector3 chestPosition)
    {
        string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: true);

        HandleDroppedItem(itemToDrop, chestPosition);
    }

    private void HandleDroppedItem(string itemToDrop, Vector3 dropLocation)
    {
        GameObject itemPrefab = itemPrefabMapper.GetItemPrefab(itemToDrop);

        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, dropLocation, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Item prefab not found for: " + itemToDrop);
        }
    }
}

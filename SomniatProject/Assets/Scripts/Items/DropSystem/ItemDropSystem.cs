using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

public class ItemDropSystem : MonoBehaviour
{
    public ItemPrefabMapper itemPrefabMapper;

    private Dictionary<int, List<ItemDropInfo>> itemDropTable;

    private List<ItemDropInfo> itemDrops = new List<ItemDropInfo>();

    List<string> usedCategories = new List<string>();

    [System.Serializable]
    public class ItemDropInfo
    {
        public int itemID;
        public string itemName;
        public float chestDropRate;
        public float enemyDropRate;
        public string category;
    }

    void Start()
    {
        itemPrefabMapper = GetComponent<ItemPrefabMapper>();
        if (itemPrefabMapper == null)
        {
            Debug.LogError("ItemPrefabMapper component not found on the same GameObject or attached to the script manually.");
            return;
        }

        LoadItemDropRatesFromCSV("Resources/Prefabs/Items/ItemDropData.csv");
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

                    if (values.Length >= 5)
                    {
                        int itemID;
                        if (int.TryParse(values[0], out itemID))
                        {
                            string itemName = values[1];

                            float chestDropRate;
                            if (float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out chestDropRate))
                            {
                                float enemyDropRate;
                                if (float.TryParse(values[3], NumberStyles.Float, CultureInfo.InvariantCulture, out enemyDropRate))
                                {
                                    string category = values[4];

                                    ItemDropInfo itemDropInfo = new ItemDropInfo
                                    {
                                        itemID = itemID,
                                        itemName = itemName,
                                        chestDropRate = chestDropRate,
                                        enemyDropRate = enemyDropRate,
                                        category = category
                                    };

                                    itemDrops.Add(itemDropInfo);
                                }
                                else
                                {
                                    Debug.LogWarning("Failed to parse enemyDropRate for line: " + line);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Failed to parse chestDropRate for line: " + line);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Failed to parse itemID for line: " + line);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Skipping invalid line: " + line);
                    }
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

        List<ItemDropInfo> validDrops = itemDrops
        .Where(drop => (isChestDrop ? drop.chestDropRate : drop.enemyDropRate) > 0)
        .Where(drop => !usedCategories.Contains(drop.category))
        .ToList();

        if (validDrops.Count == 0)
        {
            return null;
        }


        foreach (var itemDrop in validDrops)
        {
            float dropRate = isChestDrop ? itemDrop.chestDropRate : itemDrop.enemyDropRate;

            totalWeight += dropRate;

            if (randomValue < totalWeight)
            {
                totalWeight = 0;
                Debug.Log("Determined item " + itemDrop.itemName);
                return itemDrop.itemName;
            }
        }

        return null;
    }

    public void HandleEnemyDeath(Vector3 enemyPosition)
    {
        Debug.Log("Handling enemy death");
        int numberOfItemsToDrop = Random.Range(0, 3);

        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: false);

            if (itemToDrop != null)
            {
                Debug.Log("Item Drop is not null");
                HandleDroppedItem(itemToDrop, enemyPosition, numberOfItemsToDrop);
                usedCategories.Add(GetItemCategory(itemToDrop));
            }
        }

        usedCategories.Clear();
    }

    string GetItemCategory(string itemName)
    {

        var item = itemDrops.Find(itemDrop => itemDrop.itemName == itemName);
        if (item != null)
        {
            return item.category;
        }
        else
        {
            return "Unknown";
        }
    }

    public void HandleChestOpen(Vector3 chestPosition)
    {
        string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: true);

        HandleDroppedItem(itemToDrop, chestPosition, 1);
    }

    

    private void HandleDroppedItem(string itemToDrop, Vector3 dropLocation, int itemCount)
    {
        GameObject itemPrefab = itemPrefabMapper.GetItemPrefab(itemToDrop);

        if (itemPrefab != null)
        {
            for (int i = 0; i < itemCount; i++)
            {
                GameObject instantiatedItem = Instantiate(itemPrefab, dropLocation, Quaternion.identity);

                Spell spellComponent = instantiatedItem.GetComponent<Spell>();
                if(spellComponent != null)
                {
                    spellComponent.SetDroppedState(true);
                }
                if (itemCount == 0)
                    Debug.Log("0 items were dropped");

                Debug.Log("Dropped item: " + instantiatedItem.name);
            }
        }
        else
        {
            Debug.LogWarning("Item prefab not found for: " + itemToDrop);
        }
    }
}

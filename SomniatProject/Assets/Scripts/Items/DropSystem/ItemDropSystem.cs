using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

public class ItemDropSystem : MonoBehaviour
{
    public ItemPrefabMapper itemPrefabMapper;

    private List<ItemDropInfo> itemDrops = new List<ItemDropInfo>();
    private List<string> usedCategories = new List<string>();

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

    public void HandleEnemyDeath(Vector3 enemyPosition)
    {
        int numberOfItemsToDrop = Random.Range(1, 4);

        // Create a local copy of usedCategories for this drop instance
        List<string> localUsedCategories = new List<string>(usedCategories);

        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: false, localUsedCategories);

            if (itemToDrop != null)
            {
                Debug.Log("Dropped item: " + itemToDrop);
                HandleDroppedItem(itemToDrop, enemyPosition);
                localUsedCategories.Add(itemToDrop);
            }
        }
        Debug.Log("Number of items to drop: " + numberOfItemsToDrop);
        usedCategories.AddRange(localUsedCategories);

        localUsedCategories.Clear();
    }




    string DetermineItemToDrop(List<ItemDropInfo> itemDrops, bool isChestDrop, List<string> usedCategories)
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
                usedCategories.Add(itemDrop.category);
                totalWeight = 0;
                return itemDrop.itemName;
            }
        }

        return null;
    }

    void HandleDroppedItem(string itemToDrop, Vector3 dropLocation)
    {
        if (itemToDrop == null)
        {
            Debug.Log("No item to drop.");
            return;
        }

        GameObject itemPrefab = itemPrefabMapper.GetItemPrefab(itemToDrop);

        
        if (itemPrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            Vector3 adjustedDropLocation = dropLocation + randomOffset;


            GameObject instantiatedItem = Instantiate(itemPrefab, adjustedDropLocation, Quaternion.identity);

            Rigidbody rigidbody = instantiatedItem.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
            }

            float destroyTimer = 10f;
            Destroy(instantiatedItem, destroyTimer);

            Debug.Log("Dropped item: " + instantiatedItem.name + " at position: " + adjustedDropLocation);
        }
        else
        {
            Debug.LogWarning("Item prefab not found for: " + itemToDrop);
        }
    }

}

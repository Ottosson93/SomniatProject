using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections;

public class ItemDropSystem : MonoBehaviour
{
    public ItemPrefabMapper itemPrefabMapper;

    private List<ItemDropInfo> itemDrops = new List<ItemDropInfo>();
    private List<string> usedCategories = new List<string>();

    private int numberOfItemsToDrop;

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
        numberOfItemsToDrop = Random.Range(1, 4);

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

    public void HandleChestOpen(Vector3 chestPosition)
    {
        if(this.gameObject.name == "ChestGreen")
        {
            numberOfItemsToDrop = Random.Range(0, 3);
        }
        else if(this.gameObject.name == "ChestYellow")
        {
            numberOfItemsToDrop = Random.Range(1, 4);
        }
        else if(this.gameObject.name == "ChestRed")
        {
            numberOfItemsToDrop = Random.Range(2, 5);
        }

        List<string> localUsedCategories = new List<string>(usedCategories);

        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            string itemToDrop = DetermineItemToDrop(itemDrops, isChestDrop: false, localUsedCategories);

            if (itemToDrop != null)
            {
                Debug.Log("Dropped item: " + itemToDrop);
                HandleDroppedItem(itemToDrop, chestPosition);
                localUsedCategories.Add(itemToDrop);
            }
        }
        Debug.Log("Number of items to drop: " + numberOfItemsToDrop);
        usedCategories.AddRange(localUsedCategories);

        localUsedCategories.Clear();


    }

    public void HandleBoxDrop(Vector3 dropLocation)
    {
        int numberOfCoinsToDrop = Random.Range(0, 3);

        for(int i = 0;i < numberOfCoinsToDrop;i++)
        {
            string coinToDrop = Random.Range(0, 2) == 0 ? "1 coin" : "2 coins";

            Debug.Log("Dropped coin: " + coinToDrop);
            HandleDroppedItem(coinToDrop, dropLocation);
        }
    }


    string DetermineItemToDrop(List<ItemDropInfo> itemDrops, bool isChestDrop, List<string> usedCategories)
    {
        List<ItemDropInfo> validDrops = itemDrops
            .Where(drop => (isChestDrop ? drop.chestDropRate : drop.enemyDropRate) > 0)
            .Where(drop => !usedCategories.Contains(drop.category) || drop.category == "Money")
            .ToList();

        if (validDrops.Count == 0)
        {
            return null;
        }

        float totalWeight = validDrops.Sum(drop => isChestDrop ? drop.chestDropRate : drop.enemyDropRate);
        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;

        foreach (var itemDrop in validDrops)
        {
            float dropRate = isChestDrop ? itemDrop.chestDropRate : itemDrop.enemyDropRate;
            cumulativeWeight += dropRate;

            if (randomValue <= cumulativeWeight)
            {
                if (itemDrop.category != "Money" || !usedCategories.Contains("Money"))
                {
                    usedCategories.Add(itemDrop.category);
                }
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
                rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rigidbody.velocity = Vector3.zero;
            }

            float destroyTimer = 10f;
            Destroy(instantiatedItem, destroyTimer);

            Debug.Log("Dropped item: " + instantiatedItem.name + " at position: " + adjustedDropLocation);

            if (instantiatedItem.GetComponent<Collider>() != null)
            {
                Debug.Log("Item collider bounds: " + instantiatedItem.GetComponent<Collider>().bounds);
            }
            else
            {
                Debug.LogWarning("Item has no collider.");
            }
        }
        else
        {
            Debug.LogWarning("Item prefab not found for: " + itemToDrop);
        }
    }
}

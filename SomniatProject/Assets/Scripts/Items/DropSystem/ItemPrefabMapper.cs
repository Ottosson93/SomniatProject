using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabMapper : MonoBehaviour
{
    public Dictionary<string, GameObject> itemPrefabs = new Dictionary<string, GameObject>();

    public GameObject GetItemPrefab(string itemName)
    {
        if (itemPrefabs.ContainsKey(itemName))
        {
            return itemPrefabs[itemName];
        }
        else
        {
            Debug.LogWarning("Item not found in item prefab mapping: " + itemName);
            return null;
        }
    }
}

using UnityEngine;

public class ItemPrefabMapper : MonoBehaviour
{
    public GameObject GetItemPrefab(string itemName)
    {
        string path = "Prefabs/Items/" + itemName;
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            return prefab;
        }
        else
        {
            Debug.LogWarning("Prefab not found at path: " + path);
            return null;
        }
    }
}

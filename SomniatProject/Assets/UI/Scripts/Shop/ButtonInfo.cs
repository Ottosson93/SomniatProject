using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI QuantityText;
    public TextMeshProUGUI PriceText;
    public GameObject ShopManager;
    

    void Update()
    {
        PriceText.text = "Price: " + ShopManager.GetComponent<ShopManagerScript>().shopItems[2, itemID].ToString();
        QuantityText.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[3, itemID].ToString();
    }
}

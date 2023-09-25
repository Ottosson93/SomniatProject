using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TMPro.TMP_Text PriceText;
    public TMPro.TMP_Text QuantityText;
    public GameObject ShopManager;
    

    void Update()
    {
        PriceText.text = "Price: " + ShopManager.GetComponent<ShopManagerScript>().shopItems[2, itemID].ToString();
        QuantityText.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[3, itemID].ToString();
    }
}

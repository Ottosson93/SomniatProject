using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{

    public int[,] shopItems = new int[5, 5];
    public float currency;

    public TextMeshProUGUI currencyText;
    [SerializeField] GameObject shop;
    // Start is called before the first frame update
    void Awake()
    {
        Hide(shop);
        currencyText.text = "Currency: " + currency.ToString();

        //IDs
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;

    }

    // Update is called once per frame
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (currency >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID])
        {
            currency -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID];
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]++;
            currencyText.text = "Currency: " + currency.ToString();
            ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();

        }
    }

    public void Show(GameObject shop)
    {
        shop.SetActive(true);
    }


    public void Hide(GameObject shop)
    {
        shop.SetActive(false);
    }
}

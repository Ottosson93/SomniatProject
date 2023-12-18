using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    public static int currencyAmount;
    public TMP_Text currencyTxt;

    public RelicData[] relicItemsSO;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGO;
    public Button[] myPurchaseButtons;

    private Player player;
    [SerializeField] public GameObject shopView;

    public void Start()
    {

        currencyTxt.text = "Currency: " + currencyAmount.ToString();

        LoadPanels();
        CheckPurchasable();

        for (int i = 0; i < relicItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void CheckPurchasable()
    {
        for (int i = 0; i < relicItemsSO.Length; i++)
        {
            if (currencyAmount >= relicItemsSO[i].price)
                myPurchaseButtons[i].interactable = true;
            else
                myPurchaseButtons[i].interactable = false;
        }
    }
    public void AddCurrency()
    {
        currencyAmount += 100;
        currencyTxt.text = "Currency: " + currencyAmount.ToString();
        CheckPurchasable();
    }

    public void AddCoins(int coins)
    {
        currencyAmount += coins;
        currencyTxt.text = "Currency: " + currencyAmount.ToString();
        CheckPurchasable();
    }

    public int ReturnCurrency(int currency)
    {
        currency = currencyAmount;
        return currency;

    }

    public void LoadPanels()
    {
        for (int i = 0; i < relicItemsSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = relicItemsSO[i].title;
            shopPanels[i].descriptionTxt.text = relicItemsSO[i].description;
            shopPanels[i].priceTxt.text = relicItemsSO[i].price.ToString();
            shopPanels[i].icon.GetComponent<Image>().sprite = relicItemsSO[i].icon;
        }
    }

    public void Update()
    {
        CheckPurchasable();
        currencyTxt.text = ": " + currencyAmount.ToString();
    }

    public void PurchaseItem(int ButtonNumber)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (currencyAmount >= relicItemsSO[ButtonNumber].price)
        {
            player.Equip(relicItemsSO[ButtonNumber]);
            currencyAmount = currencyAmount - relicItemsSO[ButtonNumber].price;
            currencyTxt.text = "Currency " + currencyAmount.ToString();
            CheckPurchasable();
        }
    }

    public void Show(GameObject shopView)
    {
        shopView.SetActive(true);
    }


    public void Hide(GameObject shopView)
    {
        shopView.SetActive(false);
    }
}

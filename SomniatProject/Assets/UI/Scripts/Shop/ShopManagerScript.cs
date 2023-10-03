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
    public Relic relic;
    private Player player;
    private StatModifier statModifier;
    [SerializeField]GameObject shopView;

    public TextMeshProUGUI currencyText;
    //[SerializeField] GameObject shop;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        
        Hide(shopView);
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

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        Debug.Log("BOUGHT");

        if (currency >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID])
        {
            relic.Equip(player);
            Debug.Log("Equipped cupcakerelic");
            currency -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID];
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]++;
            currencyText.text = "Currency: " + currency.ToString();
            ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();
        }


    }

    public void BuyFirst()
    {

        if (currency >= shopItems[2, 1])
        {

            player.Dexterity.AddModifier(new StatModifier(10, StatModifier.StatModType.Flat, this, StatModifier.CharacterStatType.Dexterity));
            player.UpdateCharacterStats();
            Debug.Log("BOUGHT FIRST");
            relic.Equip(player);
            Debug.Log("Equipped cupcakerelic");
            currency -= shopItems[2, 1];
            shopItems[3, 1]++;
            currencyText.text = "Currency: " + currency.ToString();
        }


    }

    public void BuySecond()
    {
        if (currency >= shopItems[2, 2])
        {
            player.Strength.AddModifier(new StatModifier(10, StatModifier.StatModType.Flat, this, StatModifier.CharacterStatType.Strength));
            player.UpdateCharacterStats();
            Debug.Log("BOUGHT SECOND");
            currency -= shopItems[2, 2];
            shopItems[3, 2]++;
            currencyText.text = "Currency: " + currency.ToString();
        }
    }
    public void BuyThird()
    {


        if (currency >= shopItems[2, 3])
        {
            player.Intelligence.AddModifier(new StatModifier(10, StatModifier.StatModType.Flat, this, StatModifier.CharacterStatType.Intelligence));
            player.UpdateCharacterStats();
            Debug.Log("BOUGHT THIRD");
            currency -= shopItems[2, 3];
            shopItems[3, 3]++;
            currencyText.text = "Currency: " + currency.ToString();
        }




    }
    public void BuyFourth()
    {


        if (currency >= shopItems[2, 4])
        {
            Debug.Log("BOUGHT FOURTH");
            relic.Equip(player);
            currency -= shopItems[2, 4];
            shopItems[3, 4]++;
            currencyText.text = "Currency: " + currency.ToString();
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

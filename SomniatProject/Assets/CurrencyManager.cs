using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CurrencyManager : MonoBehaviour
{
    public TMP_Text currencyAmount;
    public ShopManagerScript shopManager;
    void Update()
    {
        int currency = ShopManagerScript.currencyAmount;
        currencyAmount.text = ": " + currency;
    }
}

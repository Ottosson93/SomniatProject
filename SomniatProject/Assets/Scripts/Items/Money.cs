using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Money : MonoBehaviour
{
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;
    [SerializeField] private ShopManagerScript shop;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        if (shop == null)
        {
            Debug.LogError("ShopManagerScript not assigned in the Inspector!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && shop != null)
        {
            int coinValue = ExtractCoinValue(gameObject.name);
            shop.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }

    private int ExtractCoinValue(string coinName)
    {
        Match match = Regex.Match(coinName, @"\d+");

        if (match.Success)
        {
            return int.Parse(match.Value);
        }

        return 0;
    }

}

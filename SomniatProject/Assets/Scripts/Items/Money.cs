using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Money : MonoBehaviour
{
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
    }
        private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            int coinValue = ExtractCoinValue(gameObject.name);
            //Shop.HandleMoneyInput or something
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

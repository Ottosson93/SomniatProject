using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour
{
    [SerializeField] private ShopManagerScript shop;
    [SerializeField] private GameObject shopView;
    //private bool hasBeenGenerated = false;
    public Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        shopView = GameObject.FindGameObjectWithTag("Shop");
        shop.Hide(shopView);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("PLAYER ENTERED");
            shop.Show(shopView);
            //shop.GenerateShop();            
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("EXITED SHOP");
            //player = collider.GetComponent<Transform>();
            shop.Hide(shopView);
        }

    }

}

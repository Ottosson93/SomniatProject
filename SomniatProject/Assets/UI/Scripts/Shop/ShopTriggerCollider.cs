using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour
{
    [SerializeField] private ShopManagerScript shop;
    [SerializeField] private GameObject shopView;
    public Transform player;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("PLAYER ENTERED");
            player = player.GetComponent<Transform>();
            shop.Show(shopView);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player = collider.GetComponent<Transform>();
            shop.Hide(shopView);
        }

    }
}
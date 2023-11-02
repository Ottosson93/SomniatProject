using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public RelicData[] relicItemsSO;
    public InventoryTemplate[] inventoryPanels;
    public GameObject[] inventoryPanelGO;

    private Player player;
    [SerializeField] public GameObject inventoryView;

    public void Start()
    {
        LoadPanels();

        for(int i = 0; i<relicItemsSO.Length; i++)
        {
            relicItemsSO[i].relicQuantity = 0;
        }

        for(int i=0; i<inventoryPanelGO.Length; i++)
        {
            inventoryPanelGO[i].SetActive(false);   
        }
    }

    public void Update()
    {
        UpdatePanel();
    }

    public void ResetQuantityData()
    {
        for(int i = 0; i<inventoryPanelGO.Length; i++)
        {
            relicItemsSO[i].relicQuantity = 0;
        }
    }
    public void LoadPanels()
    {
        for (int i = 0; i < relicItemsSO.Length; i++)
        {
            inventoryPanels[i].quantityText.text = relicItemsSO[i].relicQuantity.ToString();
            inventoryPanels[i].icon.GetComponent<Image>().sprite = relicItemsSO[i].icon;
        }
    }

    public void UpdatePanel()
    {
        for(int i = 0; i<relicItemsSO.Length; i++)
        {
            if(relicItemsSO[i].relicQuantity > 0)
            {
                inventoryPanels[i].quantityText.text = relicItemsSO[i].relicQuantity.ToString();
                inventoryPanelGO[i].SetActive(true);
            }
        }
    }

    public void Show(GameObject inventoryView)
    {
        inventoryView.SetActive(true);
    }


    public void Hide(GameObject inventoryView)
    {
        inventoryView.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

public class SpellSlotManager : MonoBehaviour
{
    public SpellScriptableObject[] spellsSO;
    public SpellSlotTemplate[] spellSlotPanels;
    private SpellAttackSystem spellAttackSystem;
    public GameObject[] spellSlotPanelGO;

    // Start is called before the first frame update
    void Start()
    {
        LoadPanels();
    }

    // Update is called once per frame
    void Update()
    {
        spellAttackSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<SpellAttackSystem>();
        UpdatePanel();
    }

    public void LoadPanels()
    {
        for (int i = 0; i < spellSlotPanels.Length; i++)
        {
            spellSlotPanels[i].freeChargesText.text = spellsSO[i].FreeChargeAmount.ToString(); //Add functionality for free charges here
            spellSlotPanels[i].icon.GetComponent<Image>().sprite = spellsSO[i].SpellIcon;
            spellSlotPanelGO[i].SetActive(false);
        }
    }

    public void UpdatePanel()
    {
        if (spellAttackSystem.currentSpell.SpellToCast.name == "Berserk")
        {
            spellSlotPanels[0].freeChargesText.text = spellAttackSystem.currentSpellFreeCharges.ToString();
            spellSlotPanelGO[0].SetActive(true);
            spellSlotPanelGO[1].SetActive(false);
            spellSlotPanelGO[2].SetActive(false);
        }
        else if (spellAttackSystem.currentSpell.SpellToCast.name == "Fireball")
        {
            spellSlotPanels[1].freeChargesText.text = spellAttackSystem.currentSpellFreeCharges.ToString();
            spellSlotPanelGO[1].SetActive(true);
            spellSlotPanelGO[0].SetActive(false);
            spellSlotPanelGO[2].SetActive(false);
        }
        else if (spellAttackSystem.currentSpell.SpellToCast.name == "Piercing Arrow")
        {
            spellSlotPanels[2].freeChargesText.text = spellAttackSystem.currentSpellFreeCharges.ToString();
            spellSlotPanelGO[2].SetActive(true);
            spellSlotPanelGO[0].SetActive(false);
            spellSlotPanelGO[1].SetActive(false);
        }
    }


    public void Show(GameObject spellSlotPanelGO)
    {
        spellSlotPanelGO.SetActive(true);
    }


    public void Hide(GameObject spellSlotPanelGO)
    {
        spellSlotPanelGO.SetActive(false);
    }
}

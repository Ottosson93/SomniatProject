using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplayManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public StatDisplayTemplate strengthTemplate;
    public StatDisplayTemplate dexterityTemplate;
    public StatDisplayTemplate intelligenceTemplate;

    public void Update()
    {
        LoadText();
    }

    public void LoadText()
    {
        strengthTemplate.statValue.text = playerStats.Strength.Value.ToString();
        dexterityTemplate.statValue.text = playerStats.Dexterity.Value.ToString();
        intelligenceTemplate.statValue.text = playerStats.Intelligence.Value.ToString();
    }
}

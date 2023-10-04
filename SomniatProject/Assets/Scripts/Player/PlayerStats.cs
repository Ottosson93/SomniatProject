using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats", fileName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField]
    int lucidity = 100;
    [SerializeField]
    int maxLucidity = 100;

    public void SubtractHealth(int value, Transform transform)
    {
        lucidity = Mathf.Clamp(lucidity - value, 0, maxLucidity);
    }

    public void AddHealth(int value, Transform transform)
    {
        lucidity = Mathf.Clamp(lucidity - value, 0, maxLucidity);
    }

}

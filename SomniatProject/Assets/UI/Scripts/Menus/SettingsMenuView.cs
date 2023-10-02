using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : Menu
{
    [SerializeField] private Button _backButton;
    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => MenuManager.ShowLast());
    }
}

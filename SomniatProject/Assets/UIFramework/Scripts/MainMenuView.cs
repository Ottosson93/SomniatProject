using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : Menu
{

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitGameButton;
    public override void Initialize()
    {
        _settingsButton.onClick.AddListener(() => MenuManager.Show<SettingsMenuView>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomButton : CustomUIComponent
{
    public ThemeSO theme;
    public Style style;
    public UnityEvent onClick;
    public GameObject pauseMenu;

    private Button button;
    private TextMeshProUGUI buttonText;

    public override void Setup()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnClick);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Configure()
    {
        ColorBlock cb = button.colors;
        cb.normalColor = theme.GetBackgroundColor(style);
        button.colors = cb;

        buttonText.color = theme.GetTextColor(style);
    }

    public void OnClick()
    {

        //SceneManager.LoadScene(1);
        //SceneManager.UnloadSceneAsync(1);
        //Debug.Log("You have clicked this button");
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Debug.Log("CLICKED OPEN PAUSE MENU");
    }
}

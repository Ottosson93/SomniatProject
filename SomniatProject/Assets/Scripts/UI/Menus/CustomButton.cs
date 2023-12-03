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
    public GameObject victoryMenu;
    public GameObject onDeathMenu;

    private Button button;
    private TextMeshProUGUI buttonText;

    public override void Setup()
    {
        BossBT.isAlive = true;
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        Player.isDead = false;
        BossBT.isAlive = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        //victoryMenu.SetActive(false);
        BossBT.isAlive = true;
        Player.isDead = false;
        SceneManager.LoadScene(1);
        
        
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        //victoryMenu.SetActive(false);
        BossBT.isAlive = true;
        Player.isDead = false;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

}

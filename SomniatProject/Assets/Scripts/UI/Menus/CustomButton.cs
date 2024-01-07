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
    public GameObject settingsMenu;

    [SerializeField] private PlayerStats playerStatsSO;

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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        ShopManagerScript.currencyAmount = 0;
        playerStatsSO.ResetStats();
        Player.isDead = false;
        BossBT.isAlive = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        //victoryMenu.SetActive(false);
        ShopManagerScript.currencyAmount = 0;
        playerStatsSO.ResetStats();
        BossBT.isAlive = true;
        Player.isDead = false;
        while (AudioManager.instance.enemiesEngaged > 0)
        {
            AudioManager.instance.removeEnemyEngage();
        }
        AudioManager.instance.RestartMusic(SoundEvents.instance.music);
        SceneManager.LoadScene(1);
        
        
    }

    public void Tutorial()
    {
        playerStatsSO.ResetStats();
        SceneManager.LoadScene(2);
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        //victoryMenu.SetActive(false);
        BossBT.isAlive = true;
        Player.isDead = false;
        playerStatsSO.ResetStats();
        ShopManagerScript.currencyAmount = 0;
        while (AudioManager.instance.enemiesEngaged > 0)
        {
            AudioManager.instance.removeEnemyEngage();
        }
        AudioManager.instance.RestartMusic(SoundEvents.instance.music);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager _menuManager;

    [SerializeField] private Menu _startingMenu;

    [SerializeField] private Menu[] _menus;

    [SerializeField] private PlayerStats playerStatsSO;

    private Menu _currentMenu;

    private readonly Stack<Menu> _history = new Stack<Menu>();

    public static bool GameIsPaused = false;

    private Enemy boss;
    private Player player;

    public GameObject pauseMenuView;
    public GameObject onDeathMenuView;
    public GameObject victoryMenuView;

    public void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("Escape")))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void OnDeath()
    {
        Time.timeScale = 0f;

        onDeathMenuView.SetActive(true);
        if (player != null)
        {
            onDeathMenuView.SetActive(false);
        }

    }

    void Resume()
    {
        Time.timeScale = 1f;

        if (pauseMenuView != null)
        {
            pauseMenuView.SetActive(false);
        }


        GameIsPaused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;

        if (pauseMenuView != null)
        {
            pauseMenuView.SetActive(true);
        }

        GameIsPaused = true;
    }

    public void OnVictory()
    {
        Time.timeScale = 0f;

        if (victoryMenuView != null)
        {
            victoryMenuView.SetActive(true);
        }

        GameIsPaused = true;
    }
    public static T GetMenu<T>() where T : Menu
    {
        for (int i = 0; i < _menuManager._menus.Length; i++)
        {
            if (_menuManager._menus[i] is T tMenu)
            {
                return tMenu;
            }
        }
        return null;
    }

    public static void Show<T>(bool remember = true) where T : Menu
    {
        for (int i = 0; i < _menuManager._menus.Length; i++)
        {
            if (_menuManager._menus[i] is T)
            {
                if (_menuManager._currentMenu != null)
                {
                    _menuManager._history.Push(_menuManager._currentMenu);
                }

                _menuManager._currentMenu.Hide();
            }

            _menuManager._menus[i].Show();

            _menuManager._currentMenu = _menuManager._menus[i];
        }
    }

    public static void Show(Menu menu, bool remember = true)
    {
        if (_menuManager._currentMenu != null)
        {
            if (remember)
            {
                _menuManager._history.Push(_menuManager._currentMenu);
            }

            _menuManager._currentMenu.Hide();
        }

        menu.Show();

        _menuManager._currentMenu = menu;
    }

    public static void ShowLast()
    {
        if (_menuManager._history.Count != 0)
        {
            Show(_menuManager._history.Pop(), false);
        }
    }

    private void Awake() => _menuManager = this;

    private void Start()
    {

        for (int i = 0; i < _menus.Length; i++)
        {
            if (_menus[i] != null)
            {
                _menus[i].Initialize();

                _menus[i].Hide();
            }

        }

        if (_startingMenu != null)
        {
            Show(_startingMenu, true);
        }

        if (onDeathMenuView != null)
            onDeathMenuView.SetActive(false);

        try
        {
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Enemy>();
        }
        catch
        {

        }

    }

    private void Update()
    {

        Debug.Log("Boss detected");
        player = GetComponent<Player>();

        if (Player.isDead == true)
        {
            OnDeath();
        }

        if (boss == null)
        {
            playerStatsSO.ResetStats();
            OnVictory();
        }
        
        if (victoryMenuView != null && boss != null)
        {

            victoryMenuView.SetActive(false);
        }
        Debug.Log("Time: " + Time.timeScale);
    }

}

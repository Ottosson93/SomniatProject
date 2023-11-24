using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager _menuManager;

    [SerializeField] private Menu _startingMenu;

    [SerializeField] private Menu[] _menus;

    private Menu _currentMenu;

    private readonly Stack<Menu> _history = new Stack<Menu>();

    public static bool GameIsPaused = false;

    public GameObject pauseMenuView;
    public GameObject onDeathMenuView;

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
        onDeathMenuView.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume()
    {
        pauseMenuView.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuView.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public static T GetMenu<T>() where T : Menu
    {
        for (int i=0; i<_menuManager._menus.Length; i++)
        {
            if(_menuManager._menus[i] is T tMenu)
            {
                return tMenu;
            }
        }
        return null;
    }

    public static void Show<T>(bool remember = true) where T : Menu
    {
        for(int i=0; i<_menuManager._menus.Length; i++)
        {
            if(_menuManager._menus[i] is T)
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
        if(_menuManager._history.Count != 0)
        {
            Show(_menuManager._history.Pop(), false);
        }
    }

    private void Awake() => _menuManager = this;

    private void Start()
    {

        for(int i=0; i <_menus.Length; i++)
        {
            if(_menus[i] != null)
            {
                _menus[i].Initialize();

                _menus[i].Hide();
            }
            
        }

        if (_startingMenu != null)
        {
            Show(_startingMenu, true);
        }

        onDeathMenuView.SetActive(false);
    }

    private void Update()
    {
        if(Player.isDead == true)
        {
            OnDeath();
        }
    }

}

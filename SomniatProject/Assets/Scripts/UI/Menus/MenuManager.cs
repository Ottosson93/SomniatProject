using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager s_instance;

    [SerializeField] private Menu _startingMenu;

    [SerializeField] private Menu[] _menus;

    private Menu _currentMenu;

    private readonly Stack<Menu> _history = new Stack<Menu>();

    public static bool GameIsPaused = false;

    public GameObject pauseMenuView;

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
        for (int i=0; i<s_instance._menus.Length; i++)
        {
            if(s_instance._menus[i] is T tMenu)
            {
                return tMenu;
            }
        }
        return null;
    }

    public static void Show<T>(bool remember = true) where T : Menu
    {
        for(int i=0; i<s_instance._menus.Length; i++)
        {
            if(s_instance._menus[i] is T)
            {
                if (s_instance._currentMenu != null)
                {
                    s_instance._history.Push(s_instance._currentMenu);
                }

                s_instance._currentMenu.Hide();
            }

            s_instance._menus[i].Show();

            s_instance._currentMenu = s_instance._menus[i];
        }
    }

    public static void Show(Menu menu, bool remember = true)
    {
        if (s_instance._currentMenu != null)
        {
            if (remember)
            {
                s_instance._history.Push(s_instance._currentMenu);
            }

            s_instance._currentMenu.Hide();
        }

        menu.Show();

        s_instance._currentMenu = menu;
    }

    public static void ShowLast()
    {
        if(s_instance._history.Count != 0)
        {
            Show(s_instance._history.Pop(), false);
        }
    }

    private void Awake() => s_instance = this;

    private void Start()
    {
        for(int i=0; i <_menus.Length; i++)
        {
            _menus[i].Initialize();

            _menus[i].Hide();
        }

        if(_startingMenu != null)
        {
            Show(_startingMenu, true);
        }
    }

}

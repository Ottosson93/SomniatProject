using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager _menuManager;

    [SerializeField] private PlayerStats playerStatsSO;

    public static bool GameIsPaused = false;

    private Enemy boss;
    private Player player;

    public GameObject pauseMenuView;
    public GameObject onDeathMenuView;
    public GameObject victoryMenuView;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Enemy>();
        Time.timeScale = 1f;

        Debug.Log("Player: " + player);
        Debug.Log("Boss: " + boss);

        onDeathMenuView.SetActive(false);
    }

    private void Awake() => _menuManager = this;

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


    private void LateUpdate()
    {
        Debug.Log("Boss detected");
        if (Player.isDead == true)
        {
            OnDeath();
        }

        if (BossBT.isAlive == false)
        {
            playerStatsSO.ResetStats();
            OnVictory();
        }

        if (victoryMenuView != null && BossBT.isAlive == true)
        {
            victoryMenuView.SetActive(false);
        }
        Debug.Log("Time: " + Time.timeScale);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    [SerializeField]
    private Enemy boss;

    [SerializeField]
    private Image healthbarFill;

    [SerializeField]
    private GameObject healthbar;

    private float bossHealth;
    private Player player;
    private Transform playerTransform;
    private float detectionRange = 30f;
    private GameObject victoryMenu;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        victoryMenu = GameObject.FindGameObjectWithTag("VictoryMenu");
        playerTransform = player.transform;
    }
    // Update is called once per frame
    void Update()
    {
        
        CheckForPlayer();
        bossHealth = boss.current;
        healthbarFill.type = Image.Type.Filled;
        healthbarFill.fillAmount = bossHealth / boss.health;
        if (bossHealth <= 0 && victoryMenu != null)
        {
            victoryMenu.SetActive(true);
        }
    }

    private void CheckForPlayer()
    {
        if (Vector3.Distance(boss.transform.position, playerTransform.position) <= detectionRange)
        {
            healthbar.SetActive(true);
        }
        else
            healthbar.SetActive(false);
    }
}

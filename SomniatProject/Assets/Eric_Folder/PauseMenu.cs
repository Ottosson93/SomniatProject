using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PauseMenu : PauseGame
    {
        public void Update()
        {
            if (activeSceenIndex != 0)
            {
                if (inMenu == false)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Time.timeScale = 0;
                     //   Transform pauseMenu = FindObjectOfType<PauseMenu>().transform;
                       // foreach (Transform child in pauseMenu)
                        {
                     //       child.gameObject.SetActive(true);
                        }
                        inMenu = true;
                    }
                }
            }
        }
    }

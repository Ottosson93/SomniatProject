using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Eric_folder
{


    public class PauseGame : MonoBehaviour
    {
        protected int activeSceenIndex;

        protected static bool inMenu = false;
        public bool InMenu { get => inMenu; }

        void Start()
        {
           // activeSceenIndex = SceneManager.GetActiveScene().buildIndex;
        }
        public void Resume()
        {
            Time.timeScale = 1;
            Transform pauseMenu = FindObjectOfType<PauseMenu>().transform;
            Transform shop = GameObject.Find("Shop").transform;

            foreach (Transform child in pauseMenu)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in shop)
            {
                child.gameObject.SetActive(false);
            }
            inMenu = false;
        }
    }

}
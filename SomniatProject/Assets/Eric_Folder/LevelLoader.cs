using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




    public class LevelLoader : MonoBehaviour
    {
        int activeSceenIndex;
        void Start()
        {
            activeSceenIndex = SceneManager.GetActiveScene().buildIndex;
        }


        public void LoadOptions()
        {
            Debug.Log("Load options");
            Time.timeScale = 1;
            SceneManager.LoadScene("Options Menu");
        }

        public void LoadMainMenu()
        {
            Debug.Log("Load main menu");
            if (GameObject.Find("PassableObject") != null)
            {
                GameObject.Destroy(GameObject.Find("PassableObject"));
            }
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
        }

        public void LoadNextScene()
        {
            Debug.Log("Load next scene");

            if (GameObject.Find("PassableObject") != null)
            {
           //     GameObject.Find("PassableObject").GetComponent<PassingScript>().Set();
            }
            SceneManager.LoadScene(activeSceenIndex + 1);
        }

        public void LoadGameOver()
        {
            Debug.Log("Load Game Over sreen");
            SceneManager.LoadScene("Game Over");
        }

        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
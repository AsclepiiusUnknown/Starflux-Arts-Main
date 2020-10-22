using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ty
{
    public class SceneMule : MonoBehaviour
    {
        public void ChangeScene(int sceneID)
        {
            SceneManager.LoadScene(sceneID);
        }

        public void ReloadScene()
        {
            ChangeScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class MainMenuSceneChange : MonoBehaviour
    {
        public void Change()
        {
            SceneManager.LoadScene(0);
        }
    }
}


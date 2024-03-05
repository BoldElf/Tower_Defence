using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField]private GameObject PanelPause;

    public void ActivatePause()
    {
        Time.timeScale = 0f;
        PanelPause.SetActive(true);
    }


    public void OnLevelMap()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void Play()
    {
        Time.timeScale = 1f;
        PanelPause.SetActive(false);
    }
}

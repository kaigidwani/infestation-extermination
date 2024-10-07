// ===============================
// AUTHOR       : Justin Huang
// CREATE DATE  : 10/7/24
// PURPOSE      : To Manage Button Functions
// SPECIAL NOTES:
// ===============================
// Change History:
//
//==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void PauseButton()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

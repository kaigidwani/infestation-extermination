// ===============================
// AUTHOR       : Justin Huang
// CREATE DATE  : 10/7/24
// PURPOSE      : To Manage Button Functions
// SPECIAL NOTES:
// ===============================
// Change History:
//  10/21/24 - Attempted to get gamemode working with this, start wave button does nothing right now
//  10/9/24 - Added QuitButton()
//==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject startWaveButton;

    private GameObject canvas;
    private GameMode mode;
    private UIScript ui;
    private EnemyManager enemyManager;

    public GameObject StartWaveButton
    {
        get { return startWaveButton; }
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        mode = canvas.GetComponent<GameMode>();
        ui = canvas.GetComponent<UIScript>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }

    public void PauseButton()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        mode.Mode1 = Mode.Pause;
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if (ui.GameModeText.text == "Build Mode")
        {
            mode.Mode1 = Mode.BuildMode;
        }
        else
        {
            mode.Mode1 = Mode.WaveMode;
        }
    }

    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void StartWave()
    {
        mode.Mode1 = Mode.WaveMode;
        ui.UpdateGameMode();
        enemyManager.StartWave();
        startWaveButton.SetActive(false);
    }
}

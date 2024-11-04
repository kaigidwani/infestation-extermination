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
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum HotBar
{
    item1,
    none
}
public class ButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject startWaveButton;
    [SerializeField] private TextMeshProUGUI startWaveButtonText;

    private GameObject canvas;
    private GameMode mode;
    private UIScript ui;
    private EnemyManager enemyManager;

    private HotBar hotBar;
    [SerializeField] private Button[] hotBarButtons;

    bool singlePress;

    public GameObject StartWaveButton
    {
        get => startWaveButton;
    }

    public TextMeshProUGUI StartWaveButtonText
    {
        get => startWaveButtonText;
        set => startWaveButtonText = value;
    }

    public HotBar HotBar1
    {
        get => hotBar;
        set => hotBar = value;
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        mode = canvas.GetComponent<GameMode>();
        ui = canvas.GetComponent<UIScript>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        hotBar = HotBar.none;
        singlePress = false;
    }

    void Update()
    {
        if (!singlePress)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                singlePress = true;
                Item1();
            }
        }
        else
        {
            if (Input.anyKey == false)
            {
                singlePress = false;
            }
        }
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
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void StartWave()
    {
        mode.Mode1 = Mode.WaveMode;
        ui.UpdateGameMode();
        enemyManager.StartWave();
        startWaveButton.SetActive(false);
    }

    public void Item1()
    {
        if (hotBar != HotBar.item1)
        {
            for (int i = 0; i < hotBarButtons.Length; i++)
            {
                hotBarButtons[i].image.color = Color.white;
            }

            hotBar = HotBar.item1;
            hotBarButtons[0].image.color = Color.green;
        }
        else
        {
            hotBar = HotBar.none;
            hotBarButtons[0].image.color = Color.white;
        }
    }
}

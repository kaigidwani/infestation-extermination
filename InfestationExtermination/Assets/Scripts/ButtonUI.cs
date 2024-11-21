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
    item2,
    none
}
public class ButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject startWaveButton;
    [SerializeField] private TextMeshProUGUI startWaveButtonText;

    private GameObject canvas;
    private GameState state;
    private UIScript ui;
    private EnemyManager enemyManager;

    private HotBar hotBar;
    [SerializeField] private Button[] hotBarButtons;
    [SerializeField] private TextMeshProUGUI[] hotBarCosts;
    [SerializeField] private GameObject[] hotBarFilters;

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
        state = canvas.GetComponent<GameState>();
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
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                singlePress = true;
                Item2();
            }
        }
        else
        {
            if (Input.anyKey == false)
            {
                singlePress = false;
            }
        }

        if (ui != null)
        {
            //Handles colors for the long robo
            if (ui.Currency < 6)
            {
                hotBarFilters[1].SetActive(true);
                hotBarCosts[1].color = Color.red;
            }
            else
            {
                hotBarFilters[1].SetActive(false);
                hotBarCosts[1].color = Color.white;
            }

            //Handles colors for the pit-robo
            if (ui.Currency < 4)
            {
                hotBarFilters[0].SetActive(true);
                hotBarCosts[0].color = Color.red;
            }
            else
            {
                hotBarFilters[0].SetActive(false);
                hotBarCosts[0].color = Color.white;
            }
        }
    }

    // Start Screen Buttons
    public void PlayButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }

    // Pause Screen Buttons
    public void PauseButton()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        state.State1 = State.Pause;
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if (ui.GameModeText.text == "Build Mode")
        {
            state.State1 = State.Build;
        }
        else
        {
            state.State1 = State.Wave;
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void RestartButton()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
        Time.timeScale = 1f;
    }

    public void NextLevelButton()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex + 1);
    }

    // Hot Bar Buttons
    public void Item1()
    {
        if (state.State1 == State.Pause) return;

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

    public void Item2()
    {
        if (state.State1 == State.Pause) return;

        if (hotBar != HotBar.item2)
        {
            for (int i = 0; i < hotBarButtons.Length; i++)
            {
                hotBarButtons[i].image.color = Color.white;
            }

            hotBar = HotBar.item2;
            hotBarButtons[1].image.color = Color.green;
        }
        else
        {
            hotBar = HotBar.none;
            hotBarButtons[1].image.color = Color.white;
        }
    }

    // Other Buttons
    public void StartWave()
    {
        state.State1 = State.Wave;
        ui.UpdateGameMode();
        enemyManager.StartWave();
        startWaveButton.SetActive(false);
    }
}

// ===============================
// AUTHOR       : Justin Huang
// CREATE DATE  : 9/23/24?
// PURPOSE      : To Manage Health and Currency Display Numbers
// SPECIAL NOTES:
// ===============================
// Change History:
//  10/31/24 - Adding some other stuff, forgot to update some other stuff up here, oof!
//  10/21/24 - Attempted to get gamemode working with this
//  10/11/24 - Made the methods public
//==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class UIScript : MonoBehaviour
{
    // Variables
    private int health = 0;
    private int currency = 0;
    private bool lose = false;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameWin;

    private GameObject canvas;
    private GameMode mode;
    private EnemyManager enemyManager;

    // Properties
    public TextMeshProUGUI HealthText
    {
        get => healthText;
    }

    public int Currency
    {
        get => currency;
    }

    public TextMeshProUGUI CurrencyText
    {
        get => currencyText;
    }

    public TextMeshProUGUI GameModeText
    {
        get => gameModeText;
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        mode = canvas.GetComponent<GameMode>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        health = 5;
        currency = 10;
        UpdateHealth(0);
        UpdateCurrency(0);
        UpdateGameMode();
    }

    void Update()
    {
        if (health <= 0 && lose == false)
        {
            Time.timeScale = 0f;
            lose = true;
            gameOver.SetActive(true);
        }

        if (enemyManager.WaveNumber == 6)
        {
            gameWin.SetActive(true);
        }
    }

    public void UpdateHealth(int number)
    {
        health += number;
        healthText.text = health.ToString();
    }

    public void UpdateCurrency(int number)
    {
        currency += number;
        currencyText.text = currency.ToString();
    }

    public void UpdateGameMode()
    {
        if (mode.Mode1 == Mode.BuildMode)
        {
            gameModeText.text = "Build Mode";
        }
        else if (mode.Mode1 == Mode.WaveMode)
        {
            gameModeText.text = "Wave " + enemyManager.WaveNumber;
        }
    }
}

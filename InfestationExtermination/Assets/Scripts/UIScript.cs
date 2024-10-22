// ===============================
// AUTHOR       : Justin Huang
// CREATE DATE  : 9/23/24?
// PURPOSE      : To Manage Health and Currency Display Numbers
// SPECIAL NOTES:
// ===============================
// Change History:
//  10/21/24 - Attempted to get gamemode working with this
//  10/11/24 - Made the methods public
//==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    // Variables
    private int health = 0;
    private int currency = 0;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI gameModeText;

    private GameObject canvas;
    private GameMode mode;

    // Properties
    public TextMeshProUGUI HealthText
    {
        get => healthText;
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

        health = 5;
        currency = 10;
        UpdateHealth(0);
        UpdateCurrency(0);
        UpdateGameMode();
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
            gameModeText.text = "Wave 0";
        }
    }
}

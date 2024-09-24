using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int health = 0;
    private int currency = 0;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI currencyText;

    void Start()
    {
        health = 100;
        currency = 10;
        UpdateHealth(0);
        UpdateCurrency(0);
    }

    void UpdateHealth(int number)
    {
        health += number;
        healthText.text = health.ToString();
    }

    void UpdateCurrency(int number)
    {
        currency += number;
        currencyText.text = currency.ToString();
    }
}

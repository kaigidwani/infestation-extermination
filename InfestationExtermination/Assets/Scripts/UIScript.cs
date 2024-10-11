// ===============================
// AUTHOR       : Justin Huang
// CREATE DATE  : 9/23/24?
// PURPOSE      : To Manage Health and Currency Display Numbers
// SPECIAL NOTES:
// ===============================
// Change History:
//  10/11/24 - Made the methods public
//==================================

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
        health = 5;
        currency = 100;
        UpdateHealth(0);
        UpdateCurrency(0);
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
}

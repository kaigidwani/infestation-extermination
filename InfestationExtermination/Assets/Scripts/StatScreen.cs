using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatScreen : MonoBehaviour
{
    private Robot selectedRobot;
    public Robot SelectedRobot
    {
        get => selectedRobot;
        set
        {
            // If there is already a selected robot turn off their radius circle
            if (selectedRobot != null)
            {
                selectedRobot.RadiusCircle.SetActive(false);
            }

            // Turn on radius circle of selected robot
            selectedRobot = value;
            selectedRobot.RadiusCircle.SetActive(true);
        }
    }

    // Stat Screen Stuff
    private GameObject statScreen;
    private TextMeshProUGUI damageText;
    private TextMeshProUGUI fireRateText;
    private TextMeshProUGUI rangeText;
    private TextMeshProUGUI upgradeTextDamage;
    private int upgradeCostDamage;
    private TextMeshProUGUI upgradeTextFireRate;
    private int upgradeCostFireRate;
    private TextMeshProUGUI upgradeTextRange;
    private int upgradeCostRange;

    private Button damageUpgradeButton;
    private Button fireRateUpgradeButton;
    private Button rangeUpgradeButton;
    private Button closeButton;

    private UIScript UIScript;
    private GameState state;

    // Start is called before the first frame update
    void Start()
    {
        // Find Scripts
        UIScript = GameObject.Find("Canvas").GetComponent<UIScript>();
        state = GameObject.Find("Canvas").GetComponent<GameState>();

        // Find Stat Screen object and its componenets
        statScreen = GameObject.Find("Stat Screen");
        damageText = GameObject.Find("damage text").GetComponent<TextMeshProUGUI>();
        fireRateText = GameObject.Find("fire rate text").GetComponent<TextMeshProUGUI>();
        rangeText = GameObject.Find("range text").GetComponent<TextMeshProUGUI>();
        upgradeTextDamage = GameObject.Find("damage upgrade cost text").GetComponent<TextMeshProUGUI>();
        upgradeTextFireRate = GameObject.Find("fire rate upgrade cost text").GetComponent<TextMeshProUGUI>();
        upgradeTextRange = GameObject.Find("range upgrade cost text").GetComponent<TextMeshProUGUI>();

        damageUpgradeButton = GameObject.Find("damage upgrade button").GetComponent<Button>();
        fireRateUpgradeButton = GameObject.Find("fire rate upgrade button").GetComponent<Button>();
        rangeUpgradeButton = GameObject.Find("range upgrade button").GetComponent<Button>();
        closeButton = GameObject.Find("close button").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedRobot == null) return;

        if (UIScript.Currency < selectedRobot.UpgradeCostDamage)
        {
            upgradeTextDamage.color = Color.red;
            damageUpgradeButton.image.color = Color.gray;
        }
        else
        {
            upgradeTextDamage.color = Color.black;
            damageUpgradeButton.image.color = Color.white;
        }

        if (UIScript.Currency < selectedRobot.UpgradeCostFireRate)
        {
            upgradeTextFireRate.color = Color.red;
            fireRateUpgradeButton.image.color = Color.gray;
        }
        else
        {
            upgradeTextFireRate.color = Color.black;
            fireRateUpgradeButton.image.color = Color.white;
        }

        if (UIScript.Currency < selectedRobot.UpgradeCostRange)
        {
            upgradeTextRange.color = Color.red;
            rangeUpgradeButton.image.color = Color.gray;
        }
        else
        {
            upgradeTextRange.color = Color.black;
            rangeUpgradeButton.image.color = Color.white;
        }
    }
}

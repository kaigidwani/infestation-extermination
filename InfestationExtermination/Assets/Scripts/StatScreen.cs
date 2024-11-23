using System;
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
    private Button closeButton;
    private TextMeshProUGUI robotName;
    private SpriteRenderer icon;

    // Damage Section
    private TextMeshProUGUI damageText;
    private TextMeshProUGUI damageUpgradeCostText;
    private TextMeshProUGUI damageUpgradeCountText;
    private TextMeshProUGUI damageUpgradeAmountText;
    private Button damageUpgradeButton;

    // Fire Rate Section
    private TextMeshProUGUI fireRateText;
    private TextMeshProUGUI fireRateUpgradeCostText;
    private TextMeshProUGUI fireRateUpgradeCountText;
    private TextMeshProUGUI fireRateUpgradeAmountText;
    private Button fireRateUpgradeButton;

    // Range Section
    private TextMeshProUGUI rangeText;
    private TextMeshProUGUI rangeUpgradeCostText;
    private TextMeshProUGUI rangeUpgradeCountText;
    private TextMeshProUGUI rangeUpgradeAmountText;
    private Button rangeUpgradeButton;
    
    // Other Scripts
    private UIScript UIScript;
    private GameState state;

    // Start is called before the first frame update
    void Start()
    {
        // Find Scripts
        UIScript = GameObject.Find("Canvas").GetComponent<UIScript>();
        state = GameObject.Find("Canvas").GetComponent<GameState>();

        // Find Stat Screen object and its componenets
        statScreen = GameObject.Find("StatScreen");
        closeButton = GameObject.Find("StatScreen/closeButton").GetComponent<Button>();
        robotName = GameObject.Find("StatScreen/robotName").GetComponent<TextMeshProUGUI>();
        icon = GameObject.Find("StatScreen/icon").GetComponent<SpriteRenderer>();

        // Damage Section
        damageText = GameObject.Find("StatScreen/damage/damageText").GetComponent<TextMeshProUGUI>();
        damageUpgradeCostText = GameObject.Find("StatScreen/damage/damageUpgradeCostText").GetComponent<TextMeshProUGUI>();
        damageUpgradeCountText = GameObject.Find("StatScreen/damage/damageUpgradeCountText").GetComponent<TextMeshProUGUI>();
        damageUpgradeAmountText = GameObject.Find("StatScreen/damage/damageUpgradeAmountText").GetComponent<TextMeshProUGUI>();
        damageUpgradeButton = GameObject.Find("StatScreen/damage/damageUpgradeButton").GetComponent<Button>();

        // Fire Rate Section
        fireRateText = GameObject.Find("StatScreen/fireRate/fireRateText").GetComponent<TextMeshProUGUI>();
        fireRateUpgradeCostText = GameObject.Find("StatScreen/fireRate/fireRateUpgradeCostText").GetComponent<TextMeshProUGUI>();
        fireRateUpgradeCountText = GameObject.Find("StatScreen/fireRate/fireRateUpgradeCountText").GetComponent<TextMeshProUGUI>();
        fireRateUpgradeAmountText = GameObject.Find("StatScreen/fireRate/fireRateUpgradeAmountText").GetComponent<TextMeshProUGUI>();
        fireRateUpgradeButton = GameObject.Find("StatScreen/fireRate/fireRateUpgradeButton").GetComponent<Button>();

        // Range Section
        rangeText = GameObject.Find("StatScreen/range/rangeText").GetComponent<TextMeshProUGUI>();
        rangeUpgradeCostText = GameObject.Find("StatScreen/range/rangeUpgradeCostText").GetComponent<TextMeshProUGUI>();
        rangeUpgradeCountText = GameObject.Find("StatScreen/range/rangeUpgradeCountText").GetComponent<TextMeshProUGUI>();
        rangeUpgradeAmountText = GameObject.Find("StatScreen/range/rangeUpgradeAmountText").GetComponent<TextMeshProUGUI>();
        rangeUpgradeButton = GameObject.Find("StatScreen/range/rangeUpgradeButton").GetComponent<Button>();
    }

    // This updates the buttons and text against current currency
    // I plan to add checks against maxed upgrades somewhere
    void Update()
    {
        if (selectedRobot == null) return;

        CheckCurrencyAmount();
    }

    // Methods
    void CheckCurrencyAmount()
    {
        // Damage Upgrade Cost Check
        if (UIScript.Currency < selectedRobot.DamageUpgradeCost)
        {
            damageUpgradeCostText.color = Color.red;
            damageUpgradeButton.image.color = Color.gray;
        }
        else
        {
            damageUpgradeCostText.color = Color.black;
            damageUpgradeButton.image.color = Color.white;
        }

        // Fire Rate Upgrade Cost Check
        if (UIScript.Currency < selectedRobot.FireRateUpgradeCost)
        {
            fireRateUpgradeCostText.color = Color.red;
            fireRateUpgradeButton.image.color = Color.gray;
        }
        else
        {
            fireRateUpgradeCostText.color = Color.black;
            fireRateUpgradeButton.image.color = Color.white;
        }

        // Range Upgrade Cost Check
        if (UIScript.Currency < selectedRobot.RangeUpgradeCost)
        {
            rangeUpgradeCostText.color = Color.red;
            rangeUpgradeButton.image.color = Color.gray;
        }
        else
        {
            rangeUpgradeCostText.color = Color.black;
            rangeUpgradeButton.image.color = Color.white;
        }
    }

    // Upon clicking a robot
    public void SetUp()
    {
        // Pull Stat Screen over
        statScreen.transform.position = new Vector3(6.6666f, 0, 0);

        // Set up Name and Icon
        robotName.text = selectedRobot.Name;
        icon.sprite = selectedRobot.GetComponent<SpriteRenderer>().sprite;

        // Conenct Buttons
        if (damageUpgradeButton.onClick != null)
        {
            damageUpgradeButton.onClick.RemoveAllListeners();
        }

        if (fireRateUpgradeButton.onClick != null)
        {
            fireRateUpgradeButton.onClick.RemoveAllListeners();
        }

        if (rangeUpgradeButton.onClick != null)
        {
            rangeUpgradeButton.onClick.RemoveAllListeners();
        }

        if (closeButton.onClick != null)
        {
            closeButton.onClick.RemoveAllListeners();
        }

        damageUpgradeButton.onClick.AddListener(selectedRobot.UpgradeDamage);
        fireRateUpgradeButton.onClick.AddListener(selectedRobot.UpgradeFireRate);
        rangeUpgradeButton.onClick.AddListener(selectedRobot.UpgradeRange);

        // Update rest of StatScreen
    }

    // "Closes" Stat Scren, actually just moves it offscreen
    public void CloseStatScreen()
    {
        if (state.State1 == State.Pause) return;

        statScreen.transform.position = new Vector3(15, 0, 0);
        selectedRobot.RadiusCircle.SetActive(false);
    }

    // Frequent Updates
    public void UpdateStatScreen()
    {
        // Damage Section
        damageText.text = "Damage: " + selectedRobot.Damage;
        damageUpgradeCostText.text = selectedRobot.DamageUpgradeCost.ToString();
        damageUpgradeCountText.text = selectedRobot.DamageUpgTimes + "/5";
        damageUpgradeAmountText.text = "+" + selectedRobot.DamageAdd;

        // FireRateSection
        fireRateText.text = "Fire Rate: " + Math.Round((1 / selectedRobot.RateOfFire), 2);
        fireRateUpgradeCostText.text = selectedRobot.FireRateUpgradeCost.ToString();
        fireRateUpgradeCountText.text = selectedRobot.RateOfFireUpgTimes + "/5";
        fireRateUpgradeAmountText.text = "-" + Math.Round(selectedRobot.RateOfFireSub, 2);

        // RangeSection
        rangeText.text = "Range: " + selectedRobot.Range;
        rangeUpgradeCostText.text = selectedRobot.RangeUpgradeCost.ToString();
        rangeUpgradeCountText.text = selectedRobot.RangeUpgTimes + "/5";
        rangeUpgradeAmountText.text = "+" + selectedRobot.RangeAdd;
    }
}

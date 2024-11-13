using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// ===============================
// AUTHOR: Kai Gidwani & Jacobe Richard
// CREATE DATE: 9/25/24
// PURPOSE: Holds all information and methods for the Robot turrets
// SPECIAL NOTES:
// ===============================
// Change History:
// 11/13/24 - Circle to represent range
// 10/28/24 - I added a line renderer some stuff needs fixing, some bugs
// 10/24/24 - Added in a function with shooting and changing the sprite when it does shoot. Should be future proof if we make another robot prefab. 
// 10/21/24 - I think that cost should be with the robot (Justin Huang)
// 10/16/24 - Rotation should be working, I do want to note that future robots should be drawn facing the same way 
//            or we can just draw the original one facing up and future ones as well, so we can remove offset B (Justin Huang)
// 10/16/24 - Added SFX
//  9/27/24 - Implemented AttemptShoot method (Kai Gidwani)
//          - Implemented Shoot method (Kai Gidwani)
//          - Added Gizmos and debug prints for debugging (Kai Gidwani)
//  9/25/24 - Created (Jacobe Richard & Kai Gidwani)
//==================================

public class Robot : MonoBehaviour
{
    // Fields

    // The cost of the turret
    [SerializeField] private int cost;

    // The amount of damage the turret does in an attack
    [SerializeField] private int damage;

    // The radius range a turret can shoot in
    [SerializeField] private int range;

    // How often a turret can shoot
    [SerializeField] private float rateOfFire;

    // Last used time
    [SerializeField] private float lastShotTime;

    // Cool down time
    [SerializeField] private float coolDown;

    // The enemy manager
    [SerializeField] private GameObject enemyManager;

    // Shoot SFX
    [SerializeField] private AudioSource shootSFX;

    // Reference to asteroid that this tower is placed onto
    private AsteroidScript asteroidReference;

    //Sprites for firing
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite shootingSprite;
    [SerializeField] private Sprite coolDownSprite;

    // Line Rendering Stuff
    private LineRenderer lineRenderer;
    private Transform[] linePositions = new Transform [2];
    private float lineFadeTime;

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

    [SerializeField] private UIScript UIScript;
    [SerializeField] private GameState state;

    //Circle for robot
    [SerializeField] private GameObject radiusOfShoot;

    // Properties

    // Getters and setter for turret cost
    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    // Getters and setters for turret damage
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    // Getters and setters for turret range
    public int Range
    {
        get { return range; }
        set { range = value; }
    }

    // Getters and setters for turret range of fire
    public float RateOfFire
    {
        get { return rateOfFire; }
        set { rateOfFire = value; }
    }

    // Getters and setters for the reference asteroid
    public AsteroidScript AsteroidReference
    {
        get { return asteroidReference; }
        set { asteroidReference = value; }
    }

    // Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager");

        lineRenderer = GetComponent<LineRenderer>();
        linePositions[0] = transform;

        statScreen = GameObject.Find("Stat Screen");
        damageText = GameObject.Find("damage text").GetComponent<TextMeshProUGUI>();
        fireRateText = GameObject.Find("fire rate text").GetComponent<TextMeshProUGUI>();
        rangeText = GameObject.Find("range text").GetComponent<TextMeshProUGUI>();
        upgradeTextDamage = GameObject.Find("damage upgrade cost text").GetComponent<TextMeshProUGUI>();
        upgradeCostDamage = 2;
        upgradeTextFireRate = GameObject.Find("fire rate upgrade cost text").GetComponent<TextMeshProUGUI>();
        upgradeCostFireRate = 2;
        upgradeTextRange = GameObject.Find("range upgrade cost text").GetComponent<TextMeshProUGUI>();
        upgradeCostRange = 2;

        damageUpgradeButton = GameObject.Find("damage upgrade button").GetComponent<Button>();
        fireRateUpgradeButton = GameObject.Find("fire rate upgrade button").GetComponent<Button>();
        rangeUpgradeButton = GameObject.Find("range upgrade button").GetComponent<Button>();
        closeButton = GameObject.Find("close button").GetComponent<Button>();

        UIScript = GameObject.Find("Canvas").GetComponent<UIScript>();
        state = GameObject.Find("Canvas").GetComponent<GameState>();

        coolDown = 999;
        lineFadeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Attempt to shoot an enemy
        AttemptShoot();

        //Counts over time
        coolDown += Time.deltaTime;

        SpriteChange();

        // Update location to asteroid reference's location
        // Sets the Z to 0 so it draws over asteroids
        transform.position = new Vector3(asteroidReference.transform.position.x,
            asteroidReference.transform.position.y,
            0);

        if (lineRenderer.enabled)
        {
            lineFadeTime += Time.deltaTime;

            if (lineFadeTime > 0.2)
            {
                lineFadeTime = 0;
                lineRenderer.enabled = false;
            }

            for (int i = 0; i < 2; i++)
            {
                if (linePositions[i] != null)
                {
                    lineRenderer.SetPosition(i, linePositions[i].position);
                }
            }
        }

        if (UIScript.Currency < upgradeCostDamage)
        {
            upgradeTextDamage.color = Color.red;
            damageUpgradeButton.image.color = Color.gray;
        }
        else
        {
            upgradeTextDamage.color = Color.black;
            damageUpgradeButton.image.color = Color.white;
        }

        if (UIScript.Currency < upgradeCostFireRate)
        {
            upgradeTextFireRate.color = Color.red;
            fireRateUpgradeButton.image.color = Color.gray;
        }
        else
        {
            upgradeTextFireRate.color = Color.black;
            fireRateUpgradeButton.image.color = Color.white;
        }

        if (UIScript.Currency < upgradeCostRange)
        {
            upgradeTextRange.color = Color.red;
            rangeUpgradeButton.image.color = Color.gray;
        }
        else
        {
            upgradeTextRange.color = Color.black;
            rangeUpgradeButton.image.color = Color.white;
        }

        //Constantly updates the circle based on the radius.
        radiusOfShoot.transform.localScale = new Vector3(Range * 2, Range * 2, 1);
    }

    // Attempt to shoot an enemy in range
    void AttemptShoot()
    {
        // Check if cooldown is up with Delta Time
        if (Time.time > lastShotTime + rateOfFire)
        {
            lastShotTime = Time.time;

            // Print a debug log that we are going to try to shoot
            // Debug.Log("Attempting to shoot!");

            // If there is at least one enemy alive
            if (enemyManager.GetComponent<EnemyManager>().EnemiesList.Count > 0)
            {
                // Create a list of enemies that are within range of this turret
                List<GameObject> inRangeEnemies = new List<GameObject>();

                // Find each enemy that is within range and add it to the list
                foreach (GameObject currentEnemy in enemyManager.GetComponent<EnemyManager>().EnemiesList)
                {
                    // If the distance to an enemy is within the range
                    if (Vector3.Distance(this.transform.position, currentEnemy.transform.position) < range)
                    {
                        // Add the current enemy to the list of in-range enemies
                        inRangeEnemies.Add(currentEnemy);
                    }
                }


                // If there is at least 1 enemy in range
                if (inRangeEnemies.Count > 0)
                {
                    // Set the initial closest enemy to the first one
                    GameObject closestEnemy = enemyManager.GetComponent<EnemyManager>().EnemiesList[0];

                    // Set the initial closest distance to the first one
                    // This is so we don't need to redo the same math multiple times,
                    // every time we want to compare distances
                    float closestEnemyDist = Vector3.Distance(this.transform.position,
                        enemyManager.GetComponent<EnemyManager>().EnemiesList[0].transform.position);

                    // Find the closest enemy that is within range
                    foreach (GameObject currentEnemy in inRangeEnemies)
                    {
                        // Saves the distance to the current enemy in the list
                        float currentDistance = Vector3.Distance(this.transform.position,
                            currentEnemy.transform.position);

                        // If the current enemy is closer than the previously closest enemy
                        if (currentDistance < closestEnemyDist)
                        {
                            // Set the closest enemy to the current enemy
                            closestEnemy = currentEnemy;

                            // Set the closest distance to the current distance
                            closestEnemyDist = currentDistance;
                        }
                    }

                    // Call the shoot function on the nearest enemy
                    Shoot(closestEnemy);
                }
            }
        }
    }

    // Shoot an enemy to do damage to it
    void Shoot(GameObject enemy)
    {
        // Call the nearest enemy's take damage function
        enemy.GetComponent<Bug>().TakeDamage(damage);

        // Debug to show when fired
        // Debug.Log("Fired!");

        //Calculates the rotation needed for the robot based on the robot's positions (KINDA BROKEN)
        Vector3 offsetA = enemy.transform.position - transform.position;
        Quaternion offsetB = quaternion.AxisAngle(new Vector3(0, 0, 1), 90);

        //Sets that rotation
        transform.rotation = Quaternion.LookRotation(Vector3.forward, offsetA);
        transform.rotation *= offsetB;

        //Plays audio
        shootSFX.Play();

        //Resets cool down timer
        coolDown = 0;

        // Add bug to line and enable line
        linePositions[1] = enemy.transform;
        lineRenderer.enabled = true;
    }

    //Changes the sprite based on lastShotTime
    void SpriteChange()
    {
        if (coolDown > rateOfFire)
        {
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }
        else if(coolDown > shootSFX.time)
        {
            GetComponent<SpriteRenderer>().sprite = coolDownSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = shootingSprite;
        }
    }

    // Draw the Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            transform.position,
            range);
    }

    // This should pull over the stat screen and display stats
    private void OnMouseDown()
    {
        if (state.State1 == State.Pause) return;

        statScreen.transform.position = new Vector3(6.6666f, 0, 0);
        UpdateStatScreen();
        
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

        damageUpgradeButton.onClick.AddListener(UpgradeDamage);
        fireRateUpgradeButton.onClick.AddListener(UpgradeFireRate);
        rangeUpgradeButton.onClick.AddListener(UpgradeRange);
        closeButton.onClick.AddListener(CloseStatScreen);
    }

    private void UpdateStatScreen()
    {
        damageText.text = "Damage: " + damage;
        fireRateText.text = "Fire Rate: " + (1 / rateOfFire);
        rangeText.text = "Range: " + range;
        upgradeTextDamage.text = "" + upgradeCostDamage;
        upgradeTextFireRate.text = "" + upgradeCostFireRate;
        upgradeTextRange.text = "" + upgradeCostRange;
    }

    // Stat Screen Buttons
    public void CloseStatScreen()
    {
        if (state.State1 == State.Pause) return;

        statScreen.transform.position = new Vector3(15, 0, 0);
    }

    public void UpgradeDamage()
    {
        if (UIScript.Currency >= upgradeCostDamage)
        {
            damage += 10;
            UIScript.UpdateCurrency(-upgradeCostDamage);
            upgradeCostDamage += 2;
            UpdateStatScreen();
        }
    }

    public void UpgradeFireRate()
    {
        if (rateOfFire > 1 && UIScript.Currency >= upgradeCostFireRate)
        {
            rateOfFire -= 0.3f;
            UIScript.UpdateCurrency(-upgradeCostFireRate);
            upgradeCostFireRate += 2;
            UpdateStatScreen();
        }
    }

    public void UpgradeRange()
    {
        if (range < 10 && UIScript.Currency >= upgradeCostRange)
        {
            range += 1;
            UIScript.UpdateCurrency(-upgradeCostRange);
            upgradeCostRange += 2;
            UpdateStatScreen();
        }
    }
}

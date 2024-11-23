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
// 11/23/24 - Argh, I have not been documenting my changes. Anyways stat screen methods had been moved from here to StatScreen.cs
//          - Also, a bunch of other minor changes like naming, adding properties, or moving stuff around.
//          - Robot should now be buffed by 25% to attack, range and fire rate if on asteroid
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

    // The name of the turret
    [SerializeField] private string name;

    // The cost of the turret
    [SerializeField] private int cost;

    // The amount of damage the turret does in an attack
    [SerializeField] private float damage;
    [SerializeField] private int damageAdd;
    private int damageUpgTimes = 0;
    [SerializeField] private int damageUpgradeCost;

    // The radius range a turret can shoot in
    [SerializeField] private float range;
    [SerializeField] private int rangeAdd;
    private int rangeUpgTimes = 0;
    [SerializeField] private int rangeUpgradeCost;

    // How often a turret can shoot
    [SerializeField] private float rateOfFire;
    [SerializeField] private float rateOfFireSub;
    private int rateOfFireUpgTimes = 0;
    [SerializeField] private int fireRateUpgradeCost;

    // Last used time
    private float lastShotTime = 0;

    // Cool down time
    private float coolDown = 100;

    // Shoot SFX
    [SerializeField] private AudioSource shootSFX;

    //Sprites for firing
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite shootingSprite;
    [SerializeField] private Sprite coolDownSprite;

    // Line Rendering Stuff
    private LineRenderer lineRenderer;
    private Transform[] linePositions = new Transform [2];
    private float lineFadeTime;

    // Reference to asteroid that this tower is placed onto
    private AsteroidScript asteroidReference;

    // Other Scripts
    private GameObject enemyManager;
    private UIScript UIScript;
    private GameState state;
    private StatScreen stat;

    //Circle for robot
    [SerializeField] private GameObject radiusCircle;

    // Properties

    public string Name
    {
        get => name;
        set => name = value;
    }

    // Getters and setter for turret cost
    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    // Getters and setters for the reference asteroid
    public AsteroidScript AsteroidReference
    {
        get { return asteroidReference; }
        set { asteroidReference = value; }
    }

    // Damage Properties
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int DamageAdd { get => damageAdd; }

    public int DamageUpgTimes { get => damageUpgTimes; }

    public int DamageUpgradeCost
    {
        get => damageUpgradeCost;
        set => damageUpgradeCost = value;
    }

    // FireRate Properties
    public float RateOfFire
    {
        get { return rateOfFire; }
        set { rateOfFire = value; }
    }

    public float RateOfFireSub { get => rateOfFireSub; }

    public int RateOfFireUpgTimes { get => rateOfFireUpgTimes; }

    public int FireRateUpgradeCost
    {
        get => fireRateUpgradeCost;
        set => fireRateUpgradeCost = value;
    }

    // Range Properties
    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public int RangeAdd { get => rangeAdd; }

    public int RangeUpgTimes { get =>  rangeUpgTimes; }

    public int RangeUpgradeCost
    {
        get => rangeUpgradeCost;
        set => rangeUpgradeCost = value;
    }

    public GameObject RadiusCircle
    { 
        get => radiusCircle;
    }

    // Methods

    // Annoying had to initialize stuff here because something went wrong with start
    void Awake()
    {
        // Line Renderer Stuff
        lineRenderer = GetComponent<LineRenderer>();
        linePositions[0] = transform;
        lineFadeTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find Scripts
        enemyManager = GameObject.Find("EnemyManager");
        UIScript = GameObject.Find("Canvas").GetComponent<UIScript>();
        state = GameObject.Find("Canvas").GetComponent<GameState>();
        stat = GameObject.Find("StatScreen").GetComponent<StatScreen>();

        // if on blackhole
        if (asteroidReference.onBlackHole == true)
        {
            damage *= 1.25f;
            rateOfFire *= 0.8f;
            range *= 1.25f;
        }
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

        // If enabled shoot line that lasts at most 0.2 second
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

        //Constantly updates the circle based on the radius. 
        if (radiusCircle.activeInHierarchy)
        {
            // This is not accurate
            radiusCircle.transform.localScale = new Vector3(Range * 2.8f, Range * 2.8f, 1);
        }
    }

    // Attempt to shoot an enemy in range
    void AttemptShoot()
    {
        // Check that there is at least one enemy in range
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

                // Call the shoot function on the nearest enemy if off cooldown
                if (Time.time > lastShotTime + rateOfFire)
                {
                    Shoot(closestEnemy);
                    lastShotTime = Time.time;
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

        stat.SelectedRobot = this;
        stat.SetUp();
        stat.UpdateStatScreen();
    }

    // Upgrade Methods
    public void UpgradeDamage()
    {
        if (damageUpgTimes < 5 && UIScript.Currency >= damageUpgradeCost)
        {
            damage += damageAdd;
            //Counts amount of times upgraded
            damageUpgTimes++;
            UIScript.UpdateCurrency(-damageUpgradeCost);
            damageUpgradeCost += 2;
            stat.UpdateStatScreen();
        }
    }

    public void UpgradeFireRate()
    {
        if (rateOfFireUpgTimes < 5 && UIScript.Currency >= fireRateUpgradeCost)
        {
            rateOfFire -= rateOfFireSub;
            //Counts amount of times upgraded
            rateOfFireUpgTimes++;
            UIScript.UpdateCurrency(-fireRateUpgradeCost);
            fireRateUpgradeCost += 2;
            stat.UpdateStatScreen();
        }
    }

    public void UpgradeRange()
    {
        if (rangeUpgTimes < 5 && UIScript.Currency >= rangeUpgradeCost)
        {
            range += rangeAdd;
            //Counts amount of times upgraded
            rangeUpgTimes++;
            UIScript.UpdateCurrency(-rangeUpgradeCost);
            rangeUpgradeCost += 2;
            stat.UpdateStatScreen();
        }
    }
}

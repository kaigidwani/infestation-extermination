using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    // Fields

    // The amount of damage the turret does in an attack
    [SerializeField] private int damage;

    // The radius range a turret can shoot in
    [SerializeField] private int range;

    // How often a turret can shoot
    [SerializeField] private float rateOfFire;

    // Last used time
    private float lastShotTime;

    // The enemy manager
    [SerializeField] private EnemyManager enemyManager;


    // Properties

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


    // Methods

    // Start is called before the first frame update
    void Start()
    {
        // Set the damage
        damage = 20;

        // Set the range
        range = 200;

        // Set the rate of fire
        rateOfFire = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Attempt to shoot an enemy
        AttemptShoot();
    }

    // Attempt to shoot an enemy in range
    void AttemptShoot()
    {
        // Check if cooldown is up with Delta Time
        if (Time.deltaTime > lastShotTime + rateOfFire)
        {
            // If there is at least one enemy alive
            if (enemyManager.EnemiesList.Count > 0)
            {
                // Set the initial closest enemy to the first one
                Bug closestEnemy = enemyManager.EnemiesList[0];

                // Set the initial closest distance to the first one
                // This is so we don't need to redo the same math multiple times,
                // every time we want to compare distances
                float closestEnemyDist = Vector3.Distance(this.transform.position,
                    enemyManager.EnemiesList[0].transform.position);


                // Find the closest enemy within range using the enemyList from the manager
                foreach (Bug currentEnemy in enemyManager.EnemiesList)
                {
                    // If the distance to an enemy is within the range
                    if (Vector3.Distance(this.transform.position, currentEnemy.transform.position) < range)
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
                }

                // Call the shoot function on the nearest enemy
                Shoot(closestEnemy);
            }
        }
    }

    // Shoot an enemy to do damage to it
    void Shoot(Bug enemy)
    {
        // Call the nearest enemy's take damage function
    }
}

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

        // Find the closest enemy within range using the enemyList from the manager

        // Call the Shoot method to attack the nearest enemy
    }

    // Shoot an enemy to do damage to it
    void Shoot(Bug enemy)
    {
        // Call the nearest enemy's take damage function
    }
}

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


    // Start is called before the first frame update
    void Start()
    {
        // Set the rate of fire to 2 seconds
        rateOfFire = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Attempt to shoot an enemy in range
    void AttemptShoot()
    {
        // Check if cooldown is up with Delta Time

        // Find the closest enemy within range
    }

    // Shoot an enemy to do damage to it
    void Shoot(Bug enemy)
    {
        // Do damage to the nearest enemy
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bug : MonoBehaviour
{
    // Fields

    // The amount of health a bug has
    [SerializeField] private int health;

    // The amount of damage a bug does in an attack
    [SerializeField] private int damage;


    // Properties

    // Getters and setters for health amount
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    // Getters and setters for damage amount
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    // Getter for bug X position
    public float X
    {
        get { return transform.position.x; }
    }

    // Getter for bug Y position
    public float Y
    {
        get { return transform.position.y; }
    }

    // Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the health
        health = 40;

        // Initalize the damage
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Takes damage from a source and reduces health
    public void TakeDamage(int damage)
    {
        // Reduce health by the amount of damage
        health -= damage;
    }
}

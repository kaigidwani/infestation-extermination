using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bug : MonoBehaviour
{
    // --- Fields ---
    [SerializeField] private GameObject thisBug; // The bug this script is attached to

    [SerializeField] private int health; // Amount of health a bug has
    [SerializeField] private int damage; // Amount of damage a bug does in an attack

    [SerializeField] private float speed = 1.0f; // Speed that the bug will move at
    [SerializeField] private List<Vector3> positions = new List<Vector3>(); // List of positions for the bug to travel to
    [SerializeField] private GameObject targetObject; // Target object of bug
    private Transform target; // Variable to hold target position
    private int positionIndex; // Current index of position list



    // --- Properties ---

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

    // --- Methods ---

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the health
        health = 40;

        // Initalize the damage
        damage = 10;

        // Set position index to 0
        positionIndex = 0;

        // Set target
        target = targetObject.transform;

    }

    // Update is called once per frame
    void Update()
    {
        // Make sure that the current position index is within range of the positions list
        if (positionIndex < positions.Count)
        {
            // Move target to new position
            targetObject.transform.position = positions[positionIndex];

            // Move position a step closer to target
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            // If the bug has reached its current target, increment the position index
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                positionIndex++;
            }
        }
        // Once the final position has been reached, take damage and destroy this bug
        else
        {
            // CANNOT FIGURE THIS OUT I AM SO SORRY
        }
    }

    // Takes damage from a source and reduces health
    public void TakeDamage(int damage)
    {
        // Reduce health by the amount of damage
        health -= damage;
    }
}

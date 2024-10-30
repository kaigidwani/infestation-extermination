using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bug : MonoBehaviour
{
    // --- Fields ---
    [SerializeField] private GameObject thisBug; // The bug this script is attached to

    [SerializeField] private int health; // Amount of health a bug has
    [SerializeField] private float healthMax; // Max amount of health a bug has at start
    [SerializeField] private int damage; // Amount of damage a bug does in an attack

    [SerializeField] private float speed = 1.0f; // Speed that the bug will move at
    [SerializeField] private List<Vector3> positions = new List<Vector3>(); // List of positions for the bug to travel to
    [SerializeField] private GameObject targetObject; // Target object of bug
    private Transform target; // Variable to hold target position
    private int positionIndex; // Current index of position list

    [SerializeField] private Image hBar; //health bar

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

    public int PositionIndex
    {
        get { return positionIndex; }
    }

    public int PositionCount
    {
        get { return positions.Count; }
    }

    // --- Methods ---

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the health and max health
        health = 40;
        healthMax = health;

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

            // Calculate the direction and angle (degrees) to the target then apply rotation
            Vector2 targetDirection = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // If the bug has reached its current target, increment the position index
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                positionIndex++;
            }
        }
    }

    // Takes damage from a source and reduces health
    public void TakeDamage(int damage)
    {
        // Reduce health by the amount of damage
        health -= damage;
        //Temp float so a fraction can be calculated. Can be removed if we change health to a float
        float fhealth = health;
        hBar.fillAmount = health / healthMax;
    }
}

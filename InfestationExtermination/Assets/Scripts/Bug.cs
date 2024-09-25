using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    // Fields

    // The amount of health a bug has
    [SerializeField] private int health;

    // The amount of damage a bug does in an attack
    [SerializeField] private int damage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Takes damage from a source and reduces health
    void TakeDamage(int damage)
    {
        // Reduce health by the amount of damage
        health -= damage;
    }
}

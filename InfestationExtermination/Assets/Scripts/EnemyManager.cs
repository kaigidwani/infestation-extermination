using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ===============================
// AUTHOR: Kai Gidwani
// CREATE DATE: 9/25/24
// PURPOSE: Manage the enemies using a manager
// SPECIAL NOTES:
// ===============================
// Change History:
//  10/11/24 - Enemy should decrease health when they reached the end (Justin Huang)
//  9/27/24 - Added Update behaviour to check if an enemy runs out of health and destroys them.
//          - Added Spawning system using variables for how many enemies to spawn and how far apart to spawn them.            
//  9/25/24 - Created
//==================================

public class EnemyManager : MonoBehaviour
{
    // === Fields ===

    // List of all bug enemies
    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

    // Prefab for bug enemy
    [SerializeField] private GameObject BugPrefab;

    // Spawn point for bugs
    [SerializeField] private Vector3 bugSpawnPoint;

    // Amount of enemies to spawn
    [SerializeField] private int amountOfEnemies;

    [SerializeField] private float distanceBetweenEnemies;

    // List of all bug enemies to be deleted
    // This prevents errors caused by deleting an item while it exists in a list being looped through
    List<GameObject> enemiesToDestroy = new List<GameObject>();
    private GameObject canvas;
    private UIScript UIScript;
    // === Properties ===

    // Gets the enemies list
    public List<GameObject> EnemiesList
    {
        get { return enemiesList; }
    }


    // === Methods ===

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started!");

        for (int i = 0; i < amountOfEnemies; i++)
        {
            // Spawn in a bug at a point incrementally upward
            EnemiesList.Add(
                Instantiate(
                    BugPrefab,
                    new Vector3(
                        bugSpawnPoint.x,
                        bugSpawnPoint.y + distanceBetweenEnemies * i,
                        bugSpawnPoint.z
                        ),
                    Quaternion.identity
                    )
                );
        }

        canvas = GameObject.Find("Canvas");
        UIScript = canvas.GetComponent<UIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // Loop through every enemy and check if its health is <= 0
        foreach (GameObject enemy in EnemiesList)
        {
            // If the enemy is out of health
            if (enemy.GetComponent<Bug>().Health <= 0)
            {
                // Add the enemy to the list of enemies to destroy
                enemiesToDestroy.Add(enemy);
            }
            if (enemy.GetComponent<Bug>().PositionIndex == enemy.GetComponent<Bug>().PositionCount)
            {
                enemiesToDestroy.Add(enemy);
                UIScript.UpdateHealth(-1);
            }
        }

        // Remove the enemies slated to be removed
        foreach (GameObject enemy in enemiesToDestroy)
        {
            // Remove the reference to the enemy
            enemiesList.Remove(enemy);

            // Delete the enemy from the scene
            Destroy(enemy);
        }
    }
}

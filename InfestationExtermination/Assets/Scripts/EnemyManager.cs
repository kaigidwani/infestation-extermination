using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// ===============================
// AUTHOR: Kai Gidwani
// CREATE DATE: 9/25/24
// PURPOSE: Manage the enemies using a manager
// SPECIAL NOTES:
// ===============================
// Change History:
//  10/31/24 - Incremental Waves (Justin Huang) Note: Not Really Good Way?
//  10/23/24 - Attempting to integrate game mode (Justin Huang)
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

    // The distance between each enemy
    [SerializeField] private float distanceBetweenEnemies;

    private int waveNumber;

    // List of all bug enemies to be deleted
    // This prevents errors caused by deleting an item while it exists in a list being looped through
    List<GameObject> enemiesToDestroy = new List<GameObject>();

    // Reference to the canvas
    private GameObject canvas;

    // Reference to the UIScript Script
    private UIScript UIScript;

    // Reference to the GameMode Script
    private GameMode GameMode;

    // Reference to the ButtonUI Script
    private ButtonUI ButtonUI;

    // === Properties ===

    // Gets the enemies list
    public List<GameObject> EnemiesList
    {
        get { return enemiesList; }
    }

    public int WaveNumber
    {
        get { return waveNumber; }
    }

    // === Methods ===

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Started!");

        canvas = GameObject.Find("Canvas");
        UIScript = canvas.GetComponent<UIScript>();
        GameMode = canvas.GetComponent<GameMode>();
        ButtonUI = canvas.GetComponent<ButtonUI>();

        waveNumber = 1;
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
                UIScript.UpdateCurrency(1);
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

        // When enemy list is empty
        if (enemiesList.Count == 0 && GameMode.Mode1 == Mode.WaveMode)
        {
            GameMode.Mode1 = Mode.BuildMode;
            UIScript.UpdateGameMode();
            waveNumber++;

            if (waveNumber < 6)
            {
                ButtonUI.StartWaveButton.SetActive(true);
                ButtonUI.StartWaveButtonText.text = "Start Wave " + waveNumber;
            }
        }
    }

    public void StartWave()
    {
        if (waveNumber == 1)
        {
            amountOfEnemies = 3;
        }
        else if (waveNumber == 2)
        {
            amountOfEnemies = 5;
        }
        else if (waveNumber == 3)
        {
            amountOfEnemies = 8;
        }
        else if (waveNumber == 4)
        {
            amountOfEnemies = 13;
        }
        else if (waveNumber == 5)
        {
            amountOfEnemies = 25;
        }

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
    }
}

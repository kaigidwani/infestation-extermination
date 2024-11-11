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
// Change History://
//  11/8/24 - Boss bug added
//  11/7/24 - Added in a wave system that can is fully reliant on lists. 
//  11/6/24 - Added in a wave system that uses lists instead of a count. Only works for wave 2 but I will change it to work for other waves. 
//  10/31/24 - Added support for different currency rewards and damage amounts
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

    // Input for enemiesList
    [System.Serializable]
    public struct Row
    {
        public List<GameObject> row;
    }

    // Wave 2 (Testing array method)
    [SerializeField] private List<Row> enemyWaves;

    // List of all bug enemies
    List<GameObject> enemiesList = new List<GameObject>();

    // List of all bug enemies to be deleted
    // This prevents errors caused by deleting an item while it exists in a list being looped through
    List<GameObject> enemiesToDestroy = new List<GameObject>();

    // Spawn point for bugs
    [SerializeField] private Vector3 bugSpawnPoint;

    // The distance between each enemy
    [SerializeField] private float distanceBetweenEnemies;

    // Check what wave it is
    private int waveNumber = 1;

    // Reference to the canvas
    private GameObject canvas;

    // Reference to the UIScript Script
    private UIScript UIScript;

    // Reference to the GameMode Script
    private GameState state;

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

    public int WaveNumbers
    {
        get { return enemyWaves.Count; }
    }

    // === Methods ===

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Started!");

        canvas = GameObject.Find("Canvas");
        UIScript = canvas.GetComponent<UIScript>();
        state = canvas.GetComponent<GameState>();
        ButtonUI = canvas.GetComponent<ButtonUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Loop through every enemy and check if its health is <= 0
        foreach (GameObject enemy in EnemiesList)
        {
            // If the enemy is out of health, destroy the enemy and reward the player
            if (enemy.GetComponent<Bug>().Health <= 0)
            {
                // Add the enemy to the list of enemies to destroy
                enemiesToDestroy.Add(enemy);

                // Reward the amount of currency to the player
                UIScript.UpdateCurrency(enemy.GetComponent<Bug>().RewardAmount);
            }

            // If the enemy reaches the end of the path, destroy the enemy and damage the player
            if (enemy.GetComponent<Bug>().PositionIndex == enemy.GetComponent<Bug>().PositionCount)
            {
                // Add the enemy to the list of enemies to destroy
                enemiesToDestroy.Add(enemy);

                // Damage the player by how much damage the enemy does
                UIScript.UpdateHealth(-enemy.GetComponent<Bug>().Damage);
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
        if (enemiesList.Count == 0 && state.State1 == State.Wave)
        {
            state.State1 = State.Build;
            UIScript.UpdateGameMode();
            waveNumber++;

            if (waveNumber < enemyWaves.Count + 1)
            {
                ButtonUI.StartWaveButton.SetActive(true);
                ButtonUI.StartWaveButtonText.text = "Start Wave " + waveNumber;
            }
        }
    }

    public void StartWave()
    {
        List<GameObject> enemyWave = enemyWaves[waveNumber - 1].row;

        for (int i = 0; i < enemyWave.Count; i++)
        {
            EnemiesList.Add(
                Instantiate(
                    enemyWave[i],
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

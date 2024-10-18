using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ===============================
// AUTHOR: Kai Gidwani
// CREATE DATE: 10/17/24
// PURPOSE: Run scripts and hold information related to black hole objects
// SPECIAL NOTES:
// ===============================
// Change History:           
//  10/17/24 - Created
//==================================

public class BlackHole : MonoBehaviour
{
    // === Fields ===

    // List of asteroids objects orbiting this black hole
    [SerializeField] List<GameObject> asteroids = new List<GameObject>();

    // Distance from center
    [SerializeField] float distanceFromCenter;

    // Rotation speed
    [SerializeField] float rotationSpeed;


    // === Methods === 

    // Start is called before the first frame update
    void Start()
    {
        // Place the given asteroids equidistant around the black hole
        InitiateAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        // If there are asteroids in the asteroid list
        if (asteroids.Count != 0)
        {
            // Rotate the asteroids

            // Loop through each asteroid in the list and spin it
            foreach (GameObject asteroid in asteroids)
            {
                // Spin the asteroid object around the center of the BlackHole object at the speed of rotationSpeed
                asteroid.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
        
    }

    // Set up the asteroids for first time usage
    private void InitiateAsteroids()
    {
        // If there are asteroids in the asteroid list
        if (asteroids.Count != 0)
        {
            // Hold the amount of degrees between each asteroid
            float asteroidSpacing = 360 / asteroids.Count;

            // For each asteroid in the list
            for (int i = 0; i < asteroids.Count; i++)
            {
                // Set the asteroid's position to the black hole's position
                asteroids[i].transform.position = transform.position;

                // Move the asteroid out from the blackhole using the distanceFromCenter
                asteroids[i].transform.SetPositionAndRotation(
                    new Vector3(asteroids[i].transform.position.x + distanceFromCenter,
                        asteroids[i].transform.position.y,
                        asteroids[i].transform.position.z),
                    Quaternion.identity
                    );

                // Rotate the asteroid around the center of the BlackHole object using the asteroidSpacing
                asteroids[i].transform.RotateAround(transform.position, Vector3.forward, asteroidSpacing * i);

            }
        }
    }
}

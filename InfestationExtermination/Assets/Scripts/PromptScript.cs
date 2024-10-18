using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PromptScript : MonoBehaviour
{
    //temp/main robot game object
    public GameObject mainRobot;

    //Asteroid position
    public Vector3 asteroidPosition;

    // To get access to UI
    private GameObject canvas;
    private UIScript UIScript;

    // Reference to asteroid this prompt is from
    private AsteroidScript asteroidReference;

    // Property for getting or setting the asteroid reference
    public AsteroidScript AsteroidReference
    {
        get { return asteroidReference; }
        set { asteroidReference = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UIScript = canvas.GetComponent<UIScript>();
    }

    //Spawns in a robot
    public void spawnRobot()
    {
        // Spawn the robot and save it as a game object
        GameObject spawnedRobot = Instantiate(mainRobot, asteroidPosition, new Quaternion());

        // Set the asteroid reference of the spawned robot to the asteroid it is located on
        spawnedRobot.GetComponent<Robot>().AsteroidReference = asteroidReference;

        UIScript.UpdateCurrency(-5);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

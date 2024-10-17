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

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UIScript = canvas.GetComponent<UIScript>();
    }

    //Spawns in a robot
    public void spawnRobot()
    {
        Instantiate(mainRobot, asteroidPosition, new Quaternion());
        UIScript.UpdateCurrency(-5);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

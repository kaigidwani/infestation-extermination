using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ================================
// AUTHOR: Emmett
// CREATE DATE: 9/?/24
// PURPOSE: Place to spawn robot
// SPECIAL NOTES:
// ===============================
// Change History:
//  11/23/24 - onBlackHole boolean added, removed some old prompt code (Maybe I shouldn't do that) - Justin Huang
//  11/20/24 - Added in the LongRobo
//  11/4/24 - Changed how spawning works - Justin Huang
//  10/19/24 - Now the prompt will spawn where the astroid was, so players can simply double click
//  10/22/24 - I dunno this didn't push. 
//==================================

public class AsteroidScript : MonoBehaviour
{
    //Canvas 
    public GameObject canvas;

    //bool to check if object on the asteroid
    public bool ifObject = false;
    public bool onBlackHole = false;

    //Vector3 of the poisiton of asteroid
    public Vector3 asteroidPosition;

    // Other scripts
    private UIScript UIScript;
    private GameState state;
    private ButtonUI buttonUI;

    // Robots
    public GameObject robot;
    public GameObject longRobot;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the canvas
        canvas = GameObject.Find("Canvas");
        state = canvas.GetComponent<GameState>();
        UIScript = canvas.GetComponent<UIScript>();
        buttonUI = canvas.GetComponent<ButtonUI>();

        //Gets the current position as well as changing the Z.
        asteroidPosition = new Vector3(transform.position.x, transform.position.y, 0); //we may need to move this to update for the moving asteroids. But we'll cross that bridge when we get to it
    }

    //For when it gets clicked 
    private void OnMouseDown()
    {
        if (UIScript.Currency >= robot.GetComponent<Robot>().Cost && buttonUI.HotBar1 == HotBar.item1 && ifObject == false && state.State1 != State.Pause)
        {
            // Spawn the robot and save it as a game object
            GameObject spawnedRobot = Instantiate(robot, asteroidPosition, new Quaternion());

            // Set the asteroid reference of the spawned robot to the asteroid it is located on
            spawnedRobot.GetComponent<Robot>().AsteroidReference = this;

            UIScript.UpdateCurrency(robot.GetComponent<Robot>().Cost * -1);

            ifObject = true;
        }
        else if (UIScript.Currency >= longRobot.GetComponent<Robot>().Cost && buttonUI.HotBar1 == HotBar.item2 && ifObject == false && state.State1 != State.Pause)
        {
            // Spawn the robot and save it as a game object
            GameObject spawnedRobot = Instantiate(longRobot, asteroidPosition, new Quaternion());

            // Set the asteroid reference of the spawned robot to the asteroid it is located on
            spawnedRobot.GetComponent<Robot>().AsteroidReference = this;

            UIScript.UpdateCurrency(longRobot.GetComponent<Robot>().Cost * -1);

            ifObject = true;
        }
    }
}

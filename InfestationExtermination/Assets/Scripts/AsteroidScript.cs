using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidScript : MonoBehaviour
{
    //button pop up prefab
    public GameObject buttonPopUp;
    //THIS IS IMPORTANT. YOU MUST CHANGE OR ELSE THE ROBOT SPAWNING WILL BREAK IF THERE ARE MULTIPLE PROMTS. THIS NAMES AND SEPERATES THE BUTTONS. MAKE SURE THE NAME IS "Promt#"
    public string promptName;
    //Canvas 
    public GameObject canvas;
    //bool to check if object on the asteroid
    public bool ifObject;
    //Vector3 of the poisiton of asteroid
    public Vector3 asteroidPosition;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the canvas
        canvas = GameObject.Find("Canvas");

        //Gets the current position as well as changing the Z.
        asteroidPosition = new Vector3(transform.position.x, transform.position.y, 0); //we may need to move this to update for the moving asteroids. But we'll cross that bridge when we get to it
    }

    // Update is called once per frame
    void Update()
    {

    }

    //For when it gets clicked 
    private void OnMouseDown()
    {
        if (ifObject == false)
        {
            //Spawns a button, searches for that button, then parents it to the canvas and gives the asteroid position. 
            Instantiate(buttonPopUp, asteroidPosition, new Quaternion());
            //Looks for the instatiated prefab
            GameObject tempPrefab = GameObject.Find("Prompt(Clone)");
            //parents it to the canvas
            tempPrefab.transform.SetParent(canvas.transform);
            //Changes the public variable asteroidposition of the tempPrefab to the astroid's asteroidPosition
            tempPrefab.GetComponent<PromptScript>().asteroidPosition = asteroidPosition;
            //Changes the tempPrefab's name to prevent errors
            tempPrefab.name = promptName;
            // Sets the AsteroidReference of the prompt to this asteroid
            tempPrefab.GetComponent<PromptScript>().AsteroidReference = this;
            ifObject = true; //Can't be clicked anymore
        }   
    }
}

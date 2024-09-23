using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstroidScript : MonoBehaviour
{
    //button pop up prefab
    public GameObject buttonPopUp;
    //THIS IS IMPORTANT. YOU MUST CHANGE OR ELSE THE ROBOT SPAWNING WILL BREAK IF THERE ARE MULTIPLE PROMTS. THIS NAMES AND SEPERATES THE BUTTONS. MAKE SURE THE NAME IS "Promt#"
    public string promptName;
    //Canvas 
    public GameObject canvas;
    //bool to check if object on the astroid
    public bool ifObject;
    //Vector3 of the poisiton of astroid
    public Vector3 astroidPosition;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the canvas
        canvas = GameObject.Find("Canvas");

        //Gets the current position as well as changing the Z.
        astroidPosition = new Vector3(transform.position.x, transform.position.y, 0); //we may need to move this to update for the moving atroids. But we'll cross that bridge when we get to it
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
            //Spawns a button, searches for that button, then parents it to the canvas and gives the astroid position. 
            Instantiate(buttonPopUp, astroidPosition, new Quaternion());
            GameObject tempPrefab = GameObject.Find("Prompt(Clone)");
            tempPrefab.transform.SetParent(canvas.transform);
            tempPrefab.GetComponent<PromptScript>().astroidPosition = astroidPosition;
            tempPrefab.name = promptName;
            ifObject = true; //Can't be clicked anymore
        }   
    }
}

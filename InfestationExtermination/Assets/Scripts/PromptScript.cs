using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PromptScript : MonoBehaviour
{
    //temp/main robot game object
    public GameObject mainRobot;

    //Astroid position
    public Vector3 astroidPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Spawns in a robot
    public void spawnRobot()
    {
        Instantiate(mainRobot, astroidPosition, new Quaternion());
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

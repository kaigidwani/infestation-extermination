using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Fields

    // List of all bug enemies
    List<Bug> EnemiesList = new List<Bug>();


    // Start is called before the first frame update
    void Start()
    {
        // === FOR DEBUGGING ===
        EnemiesList.Add(new Bug());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

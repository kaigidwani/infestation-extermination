using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Kai Gidwani
// CREATE DATE: 9/25/24
// PURPOSE: Manage the enemies using a manager
// SPECIAL NOTES:
// ===============================
// Change History:
//  9/27/24 - 
//  9/25/24 - Created
//==================================

public class EnemyManager : MonoBehaviour
{
    // Fields

    // List of all bug enemies
    List<Bug> enemiesList = new List<Bug>();


    // Properties

    // Gets the enemies list
    public List<Bug> EnemiesList
    {
        get { return enemiesList; }
    }


    // Start is called before the first frame update
    void Start()
    {
        // === FOR DEBUGGING ===
        EnemiesList.Add(new Bug());
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the health of any enemy in the list is below 0

        // If so, Remove the object then Destroy the object
    }
}

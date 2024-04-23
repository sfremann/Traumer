// --------------------------------------------------
//  file :      TeleportBack.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with teleporting back
//              to the appartment upon ending the
//              forest sequence.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling with teleporting back to the appartment upon ending the forest sequence
/// </summary>
public class TeleportBack : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private ForestManager _forestManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find manager
    /// </summary>
    private void Start()
    {
        _forestManager = GameObject.Find("Managers").transform.Find("ForestManager").gameObject.GetComponent<ForestManager>();
    }

    /// <summary>
    /// Stop forest sequence when end point reached by the player
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        _forestManager.StopForestSeq();
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
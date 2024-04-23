// --------------------------------------------------
//  file :      TestChangingRoom.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with closing the door
//              behind the player once they enter
//              the next room and destroying the
//              previous room.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script dealing with closing the door behind the player once they enter the next room and destroying the previous room
/// </summary>
public class TestChangingRoom : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private ChangingRoom _changingRoomManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find manager
    /// </summary>
    private void Start()
    {
        _changingRoomManager = GameObject.Find("Managers").transform.Find("ChangingRoomManager").gameObject.GetComponent<ChangingRoom>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Update current room and destroy previous room when the player enters the next room
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        _changingRoomManager.ChangeRoom();
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
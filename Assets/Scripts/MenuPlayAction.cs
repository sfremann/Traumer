// --------------------------------------------------
//  file :      MenuPlayAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       05/01/24
//  desc:       MenuPlayAction class definition
//              defines "play" action of the menu.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script that define the play action of the menu
/// </summary>
public class MenuPlayAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private GameObject _startMenu;
    
    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Get the startMenu GameObject
    /// </summary>    
    private void Start()
    {
        _startMenu = GameObject.Find("StartMenu");
    }

    // --------------------------------------------------

    /// <summary>
    /// Destroy the menu and quit the game
    /// </summary>
    private void OnTriggerEnter()
    {
        _startMenu.SetActive(false);
        Destroy(_startMenu);
        GameObject.Find("PlayerManager").GetComponent<PlayerController>()._canMove = true;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
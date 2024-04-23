// --------------------------------------------------
//  file :      MenuExitAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       05/01/24
//  desc:       MenuExitAction class definition
//              defines "exit" action of the menu.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script that define the exit action of the menu
/// </summary>
public class MenuExitAction : MonoBehaviour
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
        Application.Quit();
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
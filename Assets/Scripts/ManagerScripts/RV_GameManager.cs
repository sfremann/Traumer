// --------------------------------------------------
//  file :      RV_GameManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script handling the game settings.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling the game settings
/// </summary>
public class RV_GameManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool enableVR = true;

    // private variables
    //private GameObject _startMenu = null;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Check whether to use VR or not
    /// </summary>
    private void Start()
    {
        // Choose between VR and 3D player controller
        GameObject.Find("Player").SetActive(enableVR); 
        GameObject.Find("FirstPersonController").SetActive(!enableVR);

        // Find menu object when in VR
        //if (enableVR) _startMenu = GameObject.Find("StartMenu");
    }

    // --------------------------------------------------

    /// <summary>
    /// Escape triggers the exit of the application
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Check if VR is set to be used
    /// </summary>
    /// <returns>true ---> using VR</returns>
    public bool IsVR()
    {
        return enableVR;
    }

    // --------------------------------------------------

    /// <summary>
    /// Start button action
    /// </summary>
    /*
    public void StartAction()
    {
        Debug.Log("START");
        _startMenu.SetActive(false);
        Destroy(_startMenu);
        GameObject.Find("PlayerManager").GetComponent<PlayerController>()._canMove = true;
    }
    */

    // --------------------------------------------------

    /// <summary>
    /// Exit button action
    /// </summary>
    /*
    public void ExitAction()
    {
        Debug.Log("EXIT");
        _startMenu.SetActive(false);
        Destroy(_startMenu);
        Application.Quit();
    }
    */
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
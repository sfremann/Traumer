// --------------------------------------------------
//  file :      LetterAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the letter for the
//              bedroom scene.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the trigger to the end of the experience
/// </summary>
public class LetterAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private BedroomManager _bedroomManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        _bedroomManager = GameObject.Find("Managers").transform.Find("BedroomManager").gameObject.GetComponent<BedroomManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Start ending animation
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Start the ending scene
        _bedroomManager.StartEndingScene(false, true);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
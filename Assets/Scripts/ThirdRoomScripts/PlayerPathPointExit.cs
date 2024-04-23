// --------------------------------------------------
//  file :      PlayerPathPointExit.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       10/11/23
//  desc:       script used to manage the exit of a 
//              player path point (decision not made 
//              by the guide)
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script resetting forest sounds after the player returns to the guide's path
/// </summary>
public class PlayerPathPointExit : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GuidePlayer guidePlayer;
    
    // private variables
    private AudioManager _audioManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find audio manager
    /// </summary>
    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset forest sound settings when exiting player's path to return to the guide's path
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Change the mode of the guide
        guidePlayer.ChangeMode();

        // Let the forest sound come back
        _audioManager.SoundTransition("ForestSounds", 1.0f, false);
        _audioManager.SoundTransition("ForestPlayer", 1.0f, true);

        // Disable current gameobject
        gameObject.SetActive(false);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
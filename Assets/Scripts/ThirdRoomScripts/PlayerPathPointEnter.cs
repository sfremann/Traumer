// --------------------------------------------------
//  file :      PlayerPathPointEnter.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       10/11/23
//  desc:       script used to manage the enter of a 
//              player path point (decision not made 
//              by the guide)
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script managing crossing in the forest where the player can choose not to follow the guide
/// </summary>
public class PlayerPathPointEnter : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GuidePlayer guidePlayer;
    [SerializeField] private GameObject exitPathPoint;
    [SerializeField] private GameObject guidedPathPoint;

    // private variables
    private AudioManager _audioManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find audio manger
    /// </summary>
    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Change the mode of the guide when the player chooses to follow its own path and cross this point
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Change the mode of the guide
        guidePlayer.ChangeMode();

        // Change the forest sounds
        _audioManager.SoundTransition("ForestSounds", 1.0f, true);
        _audioManager.SoundTransition("ForestPlayer", 1.0f, false);

        // Enable the next pathPoint
        exitPathPoint.SetActive(true);

        // Disable current gameobject and guided one
        guidedPathPoint.SetActive(false);
        gameObject.SetActive(false);        
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
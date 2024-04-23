// --------------------------------------------------
//  file :      PhoneAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the phone for the
//              corridor scene.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the interaction with the phone in the corridor
/// </summary>
public class PhoneAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private ChangingRoom _changingRoomManager;
    private AudioManager _audioManager;
    private bool _openBathroom = true;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find manager
    /// </summary>
    private void Start()
    {
        GameObject managers = GameObject.Find("Managers");
        _changingRoomManager = managers.transform.Find("ChangingRoomManager").GetComponent<ChangingRoom>();
        _audioManager = managers.transform.Find("AudioManager").GetComponent<AudioManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Open the bathroom when interacted with, except if the bathroom is already opened
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Stop the phone ringing
        _audioManager.Stop("PhoneRinging");
        _audioManager.Play("Laughing");
        _audioManager.Play("Laughing2");

        // Open next room
        if (_openBathroom) _changingRoomManager.ChangeRoom();
    }

    // --------------------------------------------------
    
    /// <summary>
    /// Set whether the interaction with the phone should open the bathroom or not
    /// </summary>
    /// <param name="openBathroom">true ---> interacting with the phone will open the bathroom</param>
    public void SetOpenDoor(bool openBathroom)
    {
        _openBathroom = openBathroom;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
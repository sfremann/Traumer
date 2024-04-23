// --------------------------------------------------
//  file :      PlayerManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       main file for the player specific
//              actions, like freeze for instance.
// --------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

public class PlayerManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Variable declaration
    // --------------------------------------------------

    // public variables
    public GameObject fpc;
    public AudioManager audioManager;

    [HideInInspector]
    public bool isIndoor = true;
    [HideInInspector]
    public bool isInBathroom = false;

    // private variables
    private FirstPersonController _fpc;
    
    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    // Start is called before the first frame update
    private void Start()
    {
        _fpc = fpc.GetComponent<FirstPersonController>();
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    private void Update()
    {
        // in the bathroom, no headbob
        _fpc.enableHeadBob = !isInBathroom;

        // Sound management
        if (_fpc.isWalking && !isInBathroom)
        {
            if (audioManager)
            {
                if(isIndoor) audioManager.Play("IndoorFootsteps");
                else audioManager.Play("OutdoorFootsteps");
            }
        }
        else
        {
            if (audioManager)
            {
                if (isIndoor) audioManager.Stop("IndoorFootsteps");
                else audioManager.Stop("OutdoorFootsteps");
            }
        }
    }

    // --------------------------------------------------

    public void FreezePlayer(bool freeze)
    {
        //if(freeze) Debug.Log("freezing the player");
        //else Debug.Log("unfreezing the player");
        _fpc.playerCanMove = !freeze;
        _fpc.cameraCanMove = !freeze;
        GameObject.Find("FirstPersonController").GetComponent<Rigidbody>().useGravity = !freeze;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
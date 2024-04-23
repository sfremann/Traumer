// --------------------------------------------------
//  file :      FirstRoomBehavior.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       handle first room destruction.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling first room destruction
/// </summary>
public class FirstRoomBehavior : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject pictureFrame = null;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Destroy [pictureFrame] upon destruction
    /// </summary>
    private void OnDestroy()
    {
        pictureFrame.SetActive(false);
        Destroy(pictureFrame);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
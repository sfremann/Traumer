// --------------------------------------------------
//  file :      FourthRoomBehavior.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       handle fourth room destruction.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling fourth room destruction
/// </summary>
public class FourthRoomBehavior : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool destroyBathroomManager = true;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Destroy or Deactivate BathroomManager object upon destruction
    /// </summary>
    private void OnDestroy()
    {
        // Deactivate BathroomManager
        GameObject bathroomManager = GameObject.Find("Managers").transform.Find("BathroomManager").gameObject;
        bathroomManager.SetActive(false);
        if (destroyBathroomManager) Destroy(bathroomManager);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
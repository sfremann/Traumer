// --------------------------------------------------
//  file :      ThirdRoomBehavior.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       handle third room destruction.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling third room destruction
/// </summary>
public class ThirdRoomBehavior : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool destroyForestManager= true;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Destroy or Deactivate ForestManager object upon destruction
    /// </summary>
    private void OnDestroy()
    {
        GameObject forestManager = GameObject.Find("Managers").transform.Find("ForestManager").gameObject;
        forestManager.SetActive(false);
        if (destroyForestManager) Destroy(forestManager);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
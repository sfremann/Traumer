// --------------------------------------------------
//  file :      SecondRoomBehavior.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       handle second room destruction.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling second room destruction
/// </summary>
public class SecondRoomBehavior : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool destroyKitchenManager = true;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Destroy or Deactivate KitchenManager object upon destruction
    /// </summary>
    private void OnDestroy()
    {
        GameObject kitchenManager = GameObject.Find("Managers").transform.Find("KitchenManager").gameObject;
        kitchenManager.SetActive(false);
        if (destroyKitchenManager) Destroy(kitchenManager);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
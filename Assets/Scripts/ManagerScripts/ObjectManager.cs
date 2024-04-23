// --------------------------------------------------
//  file :      ObjectManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script used to destroy objects.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling the destruction of "usable" objects when needed
/// </summary>
public class ObjectManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Destroy a specific object
    /// </summary>
    /// <param name="objectToDestroy"></param>
    public void DestroyObject(GameObject objectToDestroy)
    {
        objectToDestroy.SetActive(false);
        Destroy(objectToDestroy);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
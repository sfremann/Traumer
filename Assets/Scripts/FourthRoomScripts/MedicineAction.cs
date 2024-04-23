// --------------------------------------------------
//  file :      MedicineAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the medicine for the
//              bathroom scene.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the launch of the sinking sequence in the bathroom when the medecine is interacted with
/// --- medicine replaced with a first aid kit but script remains unchanged
/// </summary>
public class MedicineAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private BathroomManager _bathroomManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        _bathroomManager = GameObject.Find("Managers").transform.Find("BathroomManager").gameObject.GetComponent<BathroomManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Start bathroom sequence
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Go to last room
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);
        _bathroomManager.StartBathroomScene();
    }
}
// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
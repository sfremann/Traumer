// --------------------------------------------------
//  file :      ShoesAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the shoes for the
//              corridor scene.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the interaction with the shoes and the start of the forest sequence in the corridor
/// </summary>
public class ShoesAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject phone;

    // private variables
    private ForestManager _forestManager;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and reset the phone's components
    /// </summary>
    private void Start()
    {
        _forestManager = GameObject.Find("Managers").transform.Find("ForestManager").gameObject.GetComponent<ForestManager>();

        // Deactivate the phone's interaction components for now
        _forestManager.AssignPhone(phone);
        phone.GetComponent<Collider>().enabled = false;
        phone.transform.Find("Fireflies").gameObject.SetActive(false);
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the forest sequence
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Start the forest scene
        _forestManager.StartForestSeq();
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
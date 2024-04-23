// --------------------------------------------------
//  file :      GuidePathPoint.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       10/11/23
//  desc:       script used to manage the enter of a 
//              guide path point (decision to follow 
//              the guide)
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the guide's path through the forest
/// </summary>
public class GuidePathPoint : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GuidePlayer guidePlayer = null;
    [SerializeField] private GameObject enterPathPoint = null;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Notify the guide when the player reaches this point
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Indicate the choice of following the guide
        guidePlayer.ChoseGuide();
        
        // Disable current gameobject and enter path point
        enterPathPoint.SetActive(false);
        gameObject.SetActive(false);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
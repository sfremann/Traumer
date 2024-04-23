// --------------------------------------------------
//  file :      RollingKnife.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script making a giant knife rolling
//              in the kitchen sequence.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script making a knife rolling 
/// --- adapted to be used on other objects but script remains unchanged
/// </summary>
public class RollingKnife : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    [SerializeField] private float rotSpeed = 360f; // (deg/sec)

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Rotate object around y-axis
    /// </summary>
    private void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Refactor the rotation speed
    /// </summary>
    /// <param name="speedFactor">factor to multiply with the current rotation speed</param>
    public void RefactorRotSpeed(float speedFactor)
    {
        rotSpeed *= speedFactor;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
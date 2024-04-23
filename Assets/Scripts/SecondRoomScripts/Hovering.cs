// --------------------------------------------------
//  file :      PlayerPathPointExit.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       10/11/23
//  desc:       script used to manage the exit of a 
//              player path point (decision not made 
//              by the guide)
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling hovering/rotation of objects in the kitchen sequence
/// </summary>
public class Hovering : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool hovering = true;
    [SerializeField] private bool rotating = true;
    [SerializeField] private bool randomSpeed = true;

    [SerializeField] private float hoverAmplitude = 1.0f;
    [SerializeField] private float hoverSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float orbitRadius = 0.01f;
    [SerializeField] private Axis hoverAxis = Axis.Y;

    [SerializeField] private float speedMin = 0.1f;
    [SerializeField] private float speedMax = 2f;

    // private variables
    private float _originValue;

    // enum
    private enum Axis
    {
        X,
        Y,
        Z
    }

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Save origin value and set hovering/rotation speed
    /// </summary>
    private void Start()
    {
        _originValue = GetAxisValue();
        rotationSpeed = (randomSpeed ? GetRandomSpeed() : rotationSpeed);
        hoverSpeed = (randomSpeed ? GetRandomSpeed() : hoverSpeed);
    }

    // --------------------------------------------------

    /// <summary>
    /// Update position using hovering and/or rotation
    /// </summary>
    private void Update()
    {
        if (hovering) UpdateHover();
        if (rotating) RotateAroundAxisWithOffset();
    }

    // --------------------------------------------------

    /// <summary>
    /// Compute a random speed value within an interval
    /// </summary>
    /// <returns>random speed value</returns>
    private float GetRandomSpeed()
    {
        return Random.Range(speedMin, speedMax);
    }

    // --------------------------------------------------

    /// <summary>
    /// Update position with hovering parameters
    /// </summary>
    private void UpdateHover()
    {
        float hoverValue = _originValue + hoverAmplitude * Mathf.Sin(Time.time * hoverSpeed);
        Vector3 newPosition = transform.position;

        switch (hoverAxis)
        {
            case Axis.X:
                newPosition.x = hoverValue;
                break;
            case Axis.Y:
                newPosition.y = hoverValue;
                break;
            case Axis.Z:
                newPosition.z = hoverValue;
                break;
        }

        transform.position = newPosition;
    }

    // --------------------------------------------------

    /// <summary>
    /// Rotate the object around an axis with an offset
    /// </summary>
    private void RotateAroundAxisWithOffset()
    {
        // Compute new position and angle
        float angle = Time.time * rotationSpeed;
        float x = Mathf.Cos(angle) * orbitRadius;
        float z = Mathf.Sin(angle) * orbitRadius;

        // Apply new position
        Vector3 newPosition = transform.position + new Vector3(x, 0, z);
        transform.position = newPosition;
    }

    // --------------------------------------------------

    /// <summary>
    /// Get the hovering value for a specific axis
    /// </summary>
    /// <returns>hovering value</returns>
    private float GetAxisValue()
    {
        return hoverAxis switch
        {
            Axis.X => transform.position.x,
            Axis.Y => transform.position.y,
            Axis.Z => transform.position.z,
            _ => 0f,
        };
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
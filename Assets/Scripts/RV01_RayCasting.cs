// --------------------------------------------------
//  file :      RV01_RayCasting.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       main file for the dialogue manager.
//              deals with the display of dialogue
//              throughout the game.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script to handle raycasting for RV01
/// </summary>
public class RV01_RayCasting : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private float rayCastDistance = 1.5f;

    // private variables
    private KitchenManager _kitchenManager;
    private Camera _playerCamera;
    private RaycastHit _hit;
    private Ray _ray;
    private LayerMask _layerGrabbable;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Get layers and managers
    /// </summary>
    private void Start()
    {
        _playerCamera = Camera.main;
        _layerGrabbable = LayerMask.GetMask("Hovering");
        _kitchenManager = GameObject.Find("Managers").transform.Find("KitchenManager").gameObject.GetComponent<KitchenManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Compute raycast and handle layers
    /// </summary>
    private void FixedUpdate()
    {
        // Raycasting creation
        _ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

        if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, rayCastDistance, _layerGrabbable))
        {
            // Indicate the detection to the Kitchen Manager
            if (_kitchenManager) _kitchenManager.RefactorMadness(_hit.collider.gameObject);
        }
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
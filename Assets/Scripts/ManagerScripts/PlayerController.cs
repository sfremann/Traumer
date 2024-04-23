// --------------------------------------------------
//  file :      PlayerController.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       main file for the RV player.
// --------------------------------------------------

using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Player controller used in VR
/// </summary>
public class PlayerController : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private Player player;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SteamVR_Action_Vector2 input;
    [SerializeField] private float speed = 1.0f;

    // private variables
    private CharacterController _characterController;
    private RV_GameManager _gameManager;
    private Vector3 _gravity = new(0f, -9.80665f, 0f);
    private bool _isInBathroom = false;

    // public variables
    public bool isIndoor = true;
    public bool _canMove = false;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Get manager and character controller
    /// </summary>
    private void Start()
    {
        _characterController = player.GetComponent<CharacterController>();
        _gameManager = GameObject.Find("RV_GameManager").GetComponent<RV_GameManager>();
        _canMove = false;
    }

    // --------------------------------------------------

    /// <summary>
    /// Update movements when in VR mode
    /// </summary>
    private void Update()
    {
        if(_gameManager.IsVR())
        {
            UpdateMovements();
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Update movements based on [input]
    /// </summary>
    private void UpdateMovements()
    {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        float coeff = Time.deltaTime;
        if(_canMove)
        {
            if (!Vector3.Equals(direction, Vector3.zero))
            {
                _characterController.Move(speed * coeff * Vector3.ProjectOnPlane(direction, Vector3.up) + _gravity * coeff);
                if(audioManager)
                {
                    if(!_isInBathroom)
                    {
                        if(isIndoor) audioManager.Play("IndoorFootsteps");
                        else audioManager.Play("OutdoorFootsteps");
                    }
                }
            }
            else
            {
                _characterController.Move(_gravity * coeff);
                if (audioManager)
                {
                    if (!_isInBathroom)
                    {
                        if (isIndoor) audioManager.Stop("IndoorFootsteps");
                        else audioManager.Stop("OutdoorFootsteps");
                    }
                }
            }
        }
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Set player's gravity to new value
    /// </summary>
    /// <param name="gravity">new gravity value</param>
    public void ChangeGravity(Vector3 gravity)
    {
        _isInBathroom = !_isInBathroom;
        _gravity = gravity;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
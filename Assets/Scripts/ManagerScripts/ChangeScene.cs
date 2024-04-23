// --------------------------------------------------
//  file :      ChangeScene.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the teleportation
//              to another scene (and the comeback).
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling teleportation between real scenes and oniric scenes
/// </summary>
public class ChangeScene : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool destroyScene = true; // true ---> destroy scene object when finished

    // private variables
    private Vector3 _originalPos;
    private Quaternion _originalRot;
    private Vector3 _originalScale;
    private float _originalSpeed;    

    private GameObject _startScene;
    private GameObject _otherScene;

    private GameObject _player;
    private FirstPersonController _fpc;
    private CharacterController _vRController;
    private bool _isVR;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Set player depending on whether VR is used or not
    /// </summary>
    private void Start()
    {
        _isVR = GameObject.Find("RV_GameManager").GetComponent<RV_GameManager>().IsVR();

        if(_isVR)
        {
            _player = GameObject.Find("Player");
            _vRController = _player.GetComponent<CharacterController>();
        }
        else
        {
            _player = GameObject.Find("FirstPersonController");
            _fpc = _player.GetComponent<FirstPersonController>();
        }
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Teleport the player from [startScene] to [otherScene]
    /// </summary>
    /// <param name="startScene">scene the player is in currently</param>
    /// <param name="otherScene">scene the player must be teleported to</param>
    /// <param name="scalePlayer">rescale factor if needed</param>
    public void TeleportToScene(GameObject startScene, GameObject otherScene, float scalePlayer = 1.0f)
    {
        // Store the scenes' info
        _startScene = startScene;
        _otherScene = otherScene;
        
        // Save the original position
        _originalPos = _player.transform.position;
        _originalRot = _player.transform.rotation;
        _originalScale = _player.transform.localScale;

        // Disable controller before teleporting the _player
        if (_isVR) _vRController.enabled = false;
        else 
        {
            _originalSpeed = _fpc.walkSpeed;
            _fpc.enabled = false;
        }

        // Set the other scene active and deactivate the reality
        _startScene.SetActive(false);
        _otherScene.SetActive(true);

        // Teleport the player
        Transform spawnPoint= _otherScene.transform.Find("SpawnPoint");
        _player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        _player.transform.localScale *= scalePlayer;

        if (_isVR) _vRController.enabled = true;
        else
        {
            _fpc.walkSpeed *= scalePlayer;
            _fpc.enabled = true;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Teleport the player back
    /// </summary>
    public void TeleportBackToStartScene()
    {
        if (_otherScene)
        {
            _otherScene.SetActive(false);
            if (destroyScene) Destroy(_otherScene);
        }
        if (_startScene) _startScene.SetActive(true);

        // Teleport back the _player
        if (_isVR) _vRController.enabled = false;
        else _fpc.enabled = false;

        _player.transform.SetPositionAndRotation(_originalPos, _originalRot);
        _player.transform.localScale = _originalScale;

        if (_isVR) _vRController.enabled = true;
        else
        {
            _fpc.walkSpeed = _originalSpeed;
            _fpc.enabled = true;
        } 
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------

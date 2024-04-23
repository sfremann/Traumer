// --------------------------------------------------
//  file :      PictureFrameAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for the action linked to the
//              picture frame object.
//              this script is attached to the
//              PictureFrame.
// --------------------------------------------------

using System.Collections;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the sequence where the player "enters" the picture frame
/// </summary>
public class PictureFrameAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject realRoom = null;
    [SerializeField] private GameObject visionRoom = null;
    [SerializeField] private float[] transitionTimes = { 1f, 1f, 1f, 1f, 1f };

    // private variables
    private Material _pictureMat;
    private Renderer _renderer;
    private Camera _playerCamera;

    private SwitchRenderPipelineAsset _renderingManager;
    private ChangeScene _changingSceneManager;
    private ChangingRoom _changingRoomManager;
    private SaturationController _saturationController;

    private Vector3 _initPos;
    private Vector3 _initScale;
    private Quaternion _initRotQuat;

    private int _transitionId = -1;

    private Coroutine _transition;

    // const
    private const float _OFFSET_TO_CAMERA = 3f;
    private const float _FINAL_SIZE = 8f;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and save initial position/scale/rotation
    /// </summary>
    private void Start()
    {
        // Save initial position, scale and rotation
        _initPos = transform.position;
        _initScale = transform.localScale;
        _initRotQuat = transform.rotation;
        _renderer = GetComponent<Renderer>();
        _pictureMat = _renderer.materials[1];

        // Find managers
        GameObject managers = GameObject.Find("Managers");
        _renderingManager = managers.transform.Find("RenderingManager").GetComponent<SwitchRenderPipelineAsset>();
        _changingSceneManager = managers.transform.Find("ChangingSceneManager").GetComponent<ChangeScene>();
        _changingRoomManager = managers.transform.Find("ChangingRoomManager").GetComponent<ChangingRoom>();
        _saturationController = managers.transform.Find("RenderingManager").GetComponent<SaturationController>();

        if (managers.transform.Find("RV_GameManager").GetComponent<RV_GameManager>().IsVR()) _playerCamera = GameObject.Find("Player").GetComponentInChildren<Camera>();
        else _playerCamera = _playerCamera = GameObject.Find("FirstPersonController").GetComponentInChildren<Camera>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the sequence when interacting with the picture frame
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Activate light around picture
        transform.Find("Light").gameObject.SetActive(true);

        // Notify Saturation Controller
        _saturationController.InPictureFrame();

        // Start transition to dream sequence
        _transition = StartCoroutine(FindingPictureTransition());        
    }

    // --------------------------------------------------

    /// <summary>
    /// Make the picture face the camera
    /// </summary>
    private void PictureFaceCamera()
    {
        transform.LookAt(transform.position - _playerCamera.transform.rotation * Vector3.forward, _playerCamera.transform.rotation * Vector3.up);

        transform.position = _playerCamera.transform.position + _playerCamera.transform.forward * _OFFSET_TO_CAMERA;
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop FindingPictureTransition and start LookingAtPictureTransition
    /// </summary>
    private void StopFindingPictureTransition()
    {
        // The picture is in front of the player
        StopCoroutine(_transition);

        // Next step
        _transition = StartCoroutine(LookingAtPictureTransition());
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop LookingAtPictureTransition and start EnteringPictureTransition
    /// </summary>
    private void StopLookingAtPictureTransition()
    {
        // Stop transition and enter the picture
        StopCoroutine(_transition);

        // Call changing scene
        _changingSceneManager.TeleportToScene(realRoom, visionRoom);

        // Use different render settings
        _renderingManager.UsePictureVisionRenderSettings();

        PictureFaceCamera();

        // Next step
        _transition = StartCoroutine(EnteringPictureTransition());
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop EnteringPictureTransition and disable picture's renderer
    /// </summary>
    private void StopEnteringPictureTransition()
    {
        StopCoroutine(_transition);

        // Make the picture completely invisible 
        _renderer.enabled = false;
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop ExitingPictureTransition and start StoringPictureTransition
    /// </summary>
    private void StopExitingPictureTransition()
    {
        StopCoroutine(_transition);

        // Return to First Room
        _changingSceneManager.TeleportBackToStartScene();
        PictureFaceCamera();

        // Reset render settings
        _saturationController.DeactivateSaturationController();
        _renderingManager.ResetRenderSettings();

        // Open the next room
        _changingRoomManager.ChangeRoom();

        // Time to move the picture away
        _transition = StartCoroutine(StoringPictureTransition());        
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop StoringPictureTransition
    /// </summary>
    private void StopStoringPictureTransition()
    {
        StopCoroutine(_transition);
        transform.SetPositionAndRotation(_initPos, _initRotQuat);

        // No more need for this script for the rest of the game
        GetComponent<PictureFrameAction>().enabled = false;
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Move the picture when found to make it face the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator FindingPictureTransition()
    {
        _transitionId++;
        float transitionStartTime = Time.timeSinceLevelLoad;
        float transitionTime = transitionTimes[_transitionId];
        float coeff; 

        while (Time.timeSinceLevelLoad < transitionTime + transitionStartTime)
        {
            coeff = Time.deltaTime;

            transform.LookAt(transform.position - _playerCamera.transform.rotation * Vector3.forward * coeff, _playerCamera.transform.rotation * Vector3.up * coeff);

            transform.position += (_playerCamera.transform.position + _playerCamera.transform.forward * _OFFSET_TO_CAMERA - transform.position) * coeff / transitionTime;

            yield return null;
        }

        StopFindingPictureTransition();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Make the picture bigger and bigger once it is in front of the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator LookingAtPictureTransition()
    {
        _transitionId++;
        float transitionTime = transitionTimes[_transitionId];
        Vector3 deltaScale = (new Vector3(_FINAL_SIZE, _FINAL_SIZE, _FINAL_SIZE) - transform.localScale) / transitionTime;

        while (transform.lossyScale.x < _FINAL_SIZE)
        {
            // Make the picture always face the camera
            PictureFaceCamera();

            // Make the picture frame look bigger and bigger
            transform.localScale += deltaScale * Time.deltaTime;

            yield return null;
        }

        StopLookingAtPictureTransition();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Make the picture fade out after the player is teleported to the picture vision
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnteringPictureTransition()
    {
        _transitionId++;
        float deltaSaturation = 1f / transitionTimes[_transitionId];

        while (_pictureMat.color.a > 0f)
        {
            // Let the picture fade out slowly  
            _pictureMat.color = new Color(_pictureMat.color.r, _pictureMat.color.g, _pictureMat.color.b, _pictureMat.color.a - deltaSaturation * Time.deltaTime);

            yield return null;
        }

        StopEnteringPictureTransition();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Activate the picture and make it fade in once the dream sequence is over
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExitingPictureTransition()
    {
        // The dream is over
        _transitionId++;

        // Activate picture again and facing camera
        PictureFaceCamera();
        
        float deltaSaturation = 1f / transitionTimes[_transitionId];

        while (_pictureMat.color.a < 1f)
        {
            // Let the picture reappear slowly
            _pictureMat.color = new Color(_pictureMat.color.r, _pictureMat.color.g, _pictureMat.color.b, _pictureMat.color.a + deltaSaturation * Time.deltaTime);

            yield return null;
        }

        StopExitingPictureTransition();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset position/rotation/scale of the picture frame after the player is teleported back to the living room
    /// </summary>
    /// <returns></returns>
    private IEnumerator StoringPictureTransition()
    {
        _transitionId++;
        float transitionTime = transitionTimes[_transitionId];
        float endTime = Time.timeSinceLevelLoad + transitionTime;
        float coeff;
        Vector3 deltaScale = (transform.localScale - _initScale) / transitionTime;

        while (Time.timeSinceLevelLoad < endTime)
        {
            coeff = Time.deltaTime;

            // Move the picture frame to the table
            transform.localScale -= deltaScale * coeff;
            transform.position += (_initPos - transform.position) * coeff / transitionTime;

            transform.Rotate(new Vector3(_initRotQuat.x - transform.rotation.x, _initRotQuat.y - transform.rotation.y, _initRotQuat.z - transform.rotation.z) * coeff / transitionTime);

            yield return null;
        }

        StopStoringPictureTransition();
        yield return null;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Notify the end of the dream sequence
    /// </summary>
    public void DreamIsOver()
    {
        // Make the picture visible again
        _renderer.enabled = true;
        _transition = StartCoroutine(ExitingPictureTransition());
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
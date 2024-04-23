// --------------------------------------------------
//  file :      SaturationController.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for tweaking saturation settings.
// --------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling saturation settings
/// </summary>
public class SaturationController : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private UniversalRendererData rendererData = null;
    [SerializeField] private Light directionalLight = null;
    [SerializeField] private PictureFrameAction pictureFrame = null;
    [SerializeField] private Material blitMat = null;
    [SerializeField] private float transitionPeriod = 7f;

    // private variables
    private float _saturationRange;
    private float _saturationOffset;

    private bool _inPictureFrame = false;
    private bool _isVR;
    private Coroutine _saturationCoroutine;

    private GlobalVolumeManager _globalVolumeManager;

    // const
    private const float _VR_SAT_MIN = -100f; // Full desaturate
    private const float _VR_SAT_MAX = -70f; // Saturation target value

    private const float _SAT_MIN = 0f;
    private const float _SAT_MAX = 0.6f;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Get managers and set saturation target values
    /// </summary>
    private void Start()
    {
        _isVR = transform.parent.Find("RV_GameManager").GetComponent<RV_GameManager>().IsVR();
        _globalVolumeManager = GetComponent<GlobalVolumeManager>();
        _globalVolumeManager.EnableDisableSaturation(_isVR);

        if (_isVR)
        {
            _saturationRange = _VR_SAT_MAX - _VR_SAT_MIN;
            _saturationOffset = _VR_SAT_MIN;
        }
        else
        {
            _saturationRange = _SAT_MAX - _SAT_MIN;
            _saturationOffset = _SAT_MIN;
        }
        ResetTransition(true);
    }

    // --------------------------------------------------

    /// <summary>
    /// End saturation transition
    /// </summary>
    /// <param name="saturate">true ---> increase saturation value</param>
    private void EndTransition(bool saturate)
    {
        StopCoroutine(_saturationCoroutine);
        ResetTransition(!saturate);
        if ((!saturate) && _inPictureFrame)
        {
            // Dream is over
            pictureFrame.DreamIsOver();
            _inPictureFrame = false;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset saturation value
    /// </summary>
    /// <param name="saturate">true ---> set value to min ; false ---> set value to max</param>
    private void ResetTransition(bool saturate)
    {
        if (_isVR) _globalVolumeManager.SetSaturationValue(saturate ? _VR_SAT_MIN : _VR_SAT_MAX);
        else
        {
            rendererData.SetDirty();
            blitMat.SetFloat("_BaseSaturation", saturate ? _SAT_MIN : _SAT_MAX);
        }
    }

    // --------------------------------------------------
    // Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Transition by saturating or desaturating
    /// </summary>
    /// <param name="saturate">true ---> increase saturation value</param>
    /// <returns></returns>
    private IEnumerator SaturationCoroutine(bool saturate)
    {
        float startTime = Time.timeSinceLevelLoad;
        ResetTransition(saturate);

        if (saturate)
        {
            float val;
            if (_isVR)
            {
                while (Time.timeSinceLevelLoad < startTime + transitionPeriod)
                {
                    // Saturate 
                    val = Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / transitionPeriod);
                    
                    _globalVolumeManager.SetSaturationValue(val * _saturationRange + _saturationOffset);

                    directionalLight.intensity *= (1 - val);

                    yield return null;
                }
            }
            else
            {
                while (Time.timeSinceLevelLoad < startTime + transitionPeriod)
                {
                    // Saturate
                    val = Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / transitionPeriod);

                    blitMat.SetFloat("_BaseSaturation", val * _saturationRange + _saturationOffset);

                    directionalLight.intensity *= (1 - val);

                    yield return null;
                }
            }
        }
        else
        {
            if (_isVR)
            {
                while (Time.timeSinceLevelLoad < startTime + transitionPeriod)
                {
                    // Desaturate
                    _globalVolumeManager.SetSaturationValue((1 - Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / transitionPeriod)) * _saturationRange + _saturationOffset);

                    yield return null;
                }
            }
            else
            {
                while (Time.timeSinceLevelLoad < startTime + transitionPeriod)
                {
                    // Desaturate
                    blitMat.SetFloat("_BaseSaturation", (1 - Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / transitionPeriod)) * _saturationRange + _saturationOffset);

                    yield return null;
                }
            }
        }

        EndTransition(saturate);
        yield return null;
    }

    // --------------------------------------------------
    // Public Methods
    // --------------------------------------------------

    /// <summary>
    /// Start saturating or desaturating
    /// </summary>
    /// <param name="saturate">true ---> increase saturation value</param>
    public void StartTransition(bool saturate)
    {
        _saturationCoroutine = StartCoroutine(SaturationCoroutine(saturate));
    }

    // --------------------------------------------------

    /// <summary>
    /// Notify arrival in picture vision
    /// </summary>
    public void InPictureFrame()
    {
        _inPictureFrame = true;
    }

    // --------------------------------------------------

    /// <summary>
    /// Deactivate saturation effects and saturation controller
    /// </summary>
    public void DeactivateSaturationController()
    {
        _globalVolumeManager.EnableDisableSaturation(false);
        GetComponent<SaturationController>().enabled = false;
    }
}

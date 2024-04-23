// --------------------------------------------------
//  file :      GlobalVolumeManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for tweaking Scene GlobalVolume
//              settings in the many sequences and
//              transitions through the experience.
// --------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling changes in GlobalVolume settings values for visual effects
/// </summary>
public class GlobalVolumeManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private float launchSeqTime = 3f;
    [SerializeField] private float redressCameraTime = 1.5f;
    [SerializeField] private float dialogueEffectTime = 1.5f;
    [SerializeField] private float forestTransitionTime = 4f;
    [SerializeField] private float bathroomTransitionTime = 6f;
    [SerializeField] private float bedroomTransitionTime = 4f;
    [SerializeField] private float creditsTransitionTime = 1f;

    // private variables
    private AudioManager _audioManager = null;
    private VolumeProfile _globalVolume;
    private Coroutine _launchSequence;
    private bool _isVR;

    // --- effects
    private LensDistortion _lensDistortion;
    private FilmGrain _filmGrain;
    private ChromaticAberration _chromaticAberration;
    private ChannelMixer _channelMixer;
    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;
    private DepthOfField _depthOfField;
    private WhiteBalance _whiteBalance;

    // --- kitchen related variables
    private float _kitchenEffectPeriod = 0.57f;
    private KitchenManager _kitchenManager;
    private Coroutine _cameraDeformationCoroutine;

    // --- forest related variables
    private ForestManager _forestManager;
    private Coroutine _transitionInOutForest;

    // --- bathroom related variables;
    private Coroutine _transitionInOutBathroom;

    // --- bedroom related variables
    private BedroomManager _bedroomManager;
    private Coroutine _transitionToEnding;
    private ParticleSystem _dancingLights;

    // const variables
    private const float _DOF_INIT = 5f;

    // --- kitchen
    private const float _KITCHEN_EFFECT_COEFF = 1.05f;
    private const float _LENS_DISTORTION = -0.4f;
    private const float _VIGNETTE_INTENSITY = 0.4f;
    private const float _RED_INTENSITY_COEFF = 0.8f;

    // --- bedroom
    private const float _POST_EXPOSURE_FINAL = 15f;
    private const float _DANCING_LIGHTS_FINAL_SIZE = 5f;  
    
    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and reset GlobalVolume settings
    /// </summary>
    private void Start()
    {
        // Get Managers
        // --- Volume profile
        _globalVolume = GetComponent<Volume>().profile;

        // --- KitchenManager for kitchen sequence
        _kitchenManager = transform.parent.Find("KitchenManager").GetComponent<KitchenManager>();

        // --- ForestManager for corridor sequence
        _forestManager = transform.parent.Find("ForestManager").GetComponent<ForestManager>();

        // --- BedroomManager for ending sequence
        _bedroomManager = transform.parent.Find("BedroomManager").GetComponent<BedroomManager>();

        // --- DialogueManager and PlayerCamera for SI28
        RV_GameManager rVGameManager = transform.parent.Find("RV_GameManager").GetComponent<RV_GameManager>();
        _isVR = rVGameManager.IsVR();

        // --- AudioManager
        _audioManager = transform.parent.Find("AudioManager").GetComponent<AudioManager>();

        // Get Features and reset them
        // --- Lens distortion
        _globalVolume.TryGet<LensDistortion>(out _lensDistortion);
        ResetLensDistortion();

        // --- Film grain
        _globalVolume.TryGet<FilmGrain>(out _filmGrain);
        ResetFilmGrain();

        // --- Chromatic Aberration
        _globalVolume.TryGet<ChromaticAberration>(out _chromaticAberration);
        ResetChromaticAberration();

        // --- Channel Mixer
        _globalVolume.TryGet<ChannelMixer>(out _channelMixer);
        ResetChannelMixer();

        // --- Vignette
        _globalVolume.TryGet<Vignette>(out _vignette);
        ResetVignette();

        // --- Color Adjustments
        _globalVolume.TryGet<ColorAdjustments>(out _colorAdjustments);
        ResetColorAdjustments();

        // --- Depth Of Field
        _globalVolume.TryGet<DepthOfField>(out _depthOfField);
        ResetDepthOfField();

        // --- White Balance
        _globalVolume.TryGet<WhiteBalance>(out _whiteBalance);
        ResetWhiteBalance();

        // Start Launch Sequence
        StartLaunchSequence();
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for LensDistortion
    /// </summary>
    private void ResetLensDistortion()
    {
        _lensDistortion.active = false;
        _lensDistortion.intensity.overrideState = true;
        _lensDistortion.intensity.value = 0f;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for FilmGrain
    /// </summary>
    private void ResetFilmGrain()
    {
        _filmGrain.active = false;
        _filmGrain.type.overrideState = true;
        _filmGrain.intensity.overrideState = true;
        _filmGrain.intensity.value = _filmGrain.intensity.max;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for ChromaticAberration
    /// </summary>
    private void ResetChromaticAberration()
    {
        _chromaticAberration.active = false;
        _chromaticAberration.intensity.overrideState = true;
        _chromaticAberration.intensity.value = 0f;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for ChannelMixer
    /// </summary>
    private void ResetChannelMixer()
    {
        _channelMixer.active = false;
        _channelMixer.redOutRedIn.overrideState = true;
        _channelMixer.redOutRedIn.value = 100f;
        _channelMixer.greenOutGreenIn.overrideState = true;
        _channelMixer.greenOutGreenIn.value = 100f;
        _channelMixer.blueOutBlueIn.overrideState = true;
        _channelMixer.blueOutBlueIn.value = 100f;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for Vignette
    /// </summary>
    private void ResetVignette()
    {
        _vignette.active = false;
        _vignette.intensity.overrideState = true;
        _vignette.intensity.value = _vignette.intensity.min;
        _vignette.smoothness.overrideState = true;
        _vignette.smoothness.value = _vignette.smoothness.min;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for ColorAdjustments
    /// </summary>
    private void ResetColorAdjustments()
    {
        _colorAdjustments.active = false;
        _colorAdjustments.postExposure.overrideState = true;
        _colorAdjustments.postExposure.value = 0f;
        _colorAdjustments.colorFilter.overrideState = false;
        _colorAdjustments.colorFilter.value = Color.black;
        _colorAdjustments.saturation.overrideState = _isVR;
        _colorAdjustments.saturation.value = 0f;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for DepthOfField
    /// </summary>
    private void ResetDepthOfField()
    {
        _depthOfField.active = false;
        _depthOfField.focusDistance.value = _DOF_INIT;
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset settings values for WhiteBalance
    /// </summary>
    private void ResetWhiteBalance()
    {
        _whiteBalance.active = false;
        _whiteBalance.temperature.overrideState = true;
        _whiteBalance.temperature.value = 0;
        _whiteBalance.tint.overrideState = true;
        _whiteBalance.tint.value = 0;
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop launch sequence transition, reset GlobalVolume settings and unlock player
    /// </summary>
    private void EndLaunchSequence()
    {
        StopCoroutine(_launchSequence);

        // Reset GlobalVolume settings
        ResetColorAdjustments();
        ResetDepthOfField();
        ResetVignette();
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the transition when moving in and out of the forest sequence
    /// </summary>
    /// <param name="intoForest">true ---> transition from appartment to forest ; false ---> transition from forest back to appartment</param>
    private void EndTransitionInOutForest(bool intoForest)
    {
        StopCoroutine(_transitionInOutForest);

        if (!intoForest)
        {
            // Leaving the forest sequence
            _forestManager.ExitForest(); 
            _colorAdjustments.colorFilter.overrideState = false;
        }

        _colorAdjustments.active = false;
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the transition when moving in and out of the sinking sequence in the bathroom
    /// </summary>
    /// <param name="intoBathroom">true ---> transition to underwater effects ; false ---> transition back to appartment settings</param>
    private void StopTransitionInOutBathroom(bool intoBathroom)
    {
        StopCoroutine(_transitionInOutBathroom);
        if (!intoBathroom)
        {
            // Reset camera settings
            ResetWhiteBalance();
            ResetDepthOfField();
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the transition to the ending of the experience and start the second part to move to the letter content
    /// </summary>
    private void EndTransitionToEnding()
    {
        StopCoroutine(_transitionToEnding);

        // Start part 2
        _transitionToEnding = StartCoroutine(TransitionToEndingPart2());
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the second part of the transition to the ending and display the letter
    /// </summary>
    /// <param name="skipLetter">true ---> move directly to the credits rolls</param>
    private void EndTransitionToEndingPart2(bool skipLetter)
    {
        StopCoroutine(_transitionToEnding);

        // Stopping the player from falling
        if (!_isVR) GameObject.Find("FirstPersonController").GetComponent<Rigidbody>().useGravity = false;

        // Start the ending
        _bedroomManager.CreditsRoll();
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop transition and disable color adjustments
    /// </summary>
    private void EndDisableColorAdjustmentsTransition()
    {
        StopCoroutine(_transitionToEnding);
        _colorAdjustments.active = false;
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Launch transition: black and blurry screen getting brighter and clearer before the experience really starts
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaunchSequence()
    {   // Enable features
        // --- Depth of Field
        _depthOfField.active = true;
        _depthOfField.focusDistance.value = _depthOfField.focusDistance.min;
        float dofVal = _DOF_INIT / launchSeqTime * 0.5f;

        // --- Vignette
        _vignette.active = true;
        _vignette.intensity.value = _vignette.intensity.max;
        _vignette.smoothness.value = _vignette.smoothness.max;
        float vignetteVal = -_vignette.intensity.value / launchSeqTime;

        // --- Color Adjustments
        _colorAdjustments.active = true;
        _colorAdjustments.colorFilter.overrideState = true;
        float colorVal = 0.5f / launchSeqTime;
        float currentColorVal;

        float coeff;

        while (_depthOfField.focusDistance.value < _DOF_INIT)
        {
            coeff = Time.deltaTime;

            // --- Depth of Field
            _depthOfField.focusDistance.value += dofVal * coeff;

            // --- Vignette
            _vignette.intensity.value += vignetteVal * coeff;

            // --- Color Adjustments
            currentColorVal = colorVal * coeff;
            _colorAdjustments.colorFilter.value = new Color(_colorAdjustments.colorFilter.value.r + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal);

            yield return null;
        }

        EndLaunchSequence();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Camera effect loop used in the kitchen sequence: visual representation of heart beats synchronized with heart beat sounds
    /// </summary>
    /// <returns></returns>
    private IEnumerator KitchenCameraEffects()
    {
        bool increaseDeformation = true;

        float effectStartTime = Time.timeSinceLevelLoad;
        float timeLimit = effectStartTime + _kitchenEffectPeriod;
        float coeff;
        
        while (true)
        {
            if (Time.timeSinceLevelLoad >= timeLimit)
            {
                // Change loop
                increaseDeformation = (!increaseDeformation);
                effectStartTime = Time.timeSinceLevelLoad;
                timeLimit = effectStartTime + _kitchenEffectPeriod;
                _kitchenManager.PlayHeartBeatSound();
            }

            // Increase or decrease deformation
            coeff = Mathf.Clamp01((Time.timeSinceLevelLoad - effectStartTime) / _kitchenEffectPeriod);
            if (!increaseDeformation) coeff = 1 - coeff;

            // Update Settings
            // --- Lens Distortion
            _lensDistortion.intensity.value = coeff * _LENS_DISTORTION;

            // --- Chromatic Aberration
            _chromaticAberration.intensity.value = coeff * _chromaticAberration.intensity.max;

            // --- Channel Mixer
            _channelMixer.redOutRedIn.value = 100 * (1 + coeff * _RED_INTENSITY_COEFF);
            _channelMixer.greenOutGreenIn.value = 200 - _channelMixer.redOutRedIn.value;
            _channelMixer.blueOutBlueIn.value = _channelMixer.greenOutGreenIn.value;

            // --- Vignette
            _vignette.intensity.value = coeff * _VIGNETTE_INTENSITY;

            yield return null;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Transition when moving in and out of the forest sequence
    /// </summary>
    /// <param name="intoForest">true ---> transition from appartment to forest ; false ---> transition from forest back to appartment</param>
    /// <returns></returns>
    private IEnumerator TransitionInOutForest(bool intoForest)
    {
        // Enable features
        // --- Color Adjustments
        _colorAdjustments.active = true;
        _colorAdjustments.colorFilter.overrideState = true;
        float colorVal = (intoForest ? 1f : -1f) / forestTransitionTime;
        float currentColorVal;

        if (intoForest) // Entering forest
        {
            _audioManager.Play("PowerDown");
            while (_colorAdjustments.colorFilter.value.r < 1f)
            {
                currentColorVal = colorVal * Time.deltaTime;
                _colorAdjustments.colorFilter.value = new Color(_colorAdjustments.colorFilter.value.r + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal);      
                
                yield return null;
            }
        }
        else // Exiting forest
        {
            _audioManager.SoundTransition("ForestSounds", 1.5f, true);
            while (_colorAdjustments.colorFilter.value.r > 0f)
            {
                currentColorVal = colorVal * Time.deltaTime;
                _colorAdjustments.colorFilter.value = new Color(_colorAdjustments.colorFilter.value.r + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal);

                yield return null;
            }
        }

        EndTransitionInOutForest(intoForest);
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the transition when moving in and out of the sinking sequence in the bathroom
    /// </summary>
    /// <param name="intoBathroom">true ---> transition to underwater effects ; false ---> transition back to appartment settings</param>
    /// <returns></returns>
    private IEnumerator TransitionInBathroom(bool intoBathroom)
    {
        // Enable features
        // --- Depth of Field
        _depthOfField.active = true;
        float dofVal = -_DOF_INIT / bathroomTransitionTime;

        // --- White Balance 
        _whiteBalance.active = true;
        float wbVal = _whiteBalance.temperature.min / bathroomTransitionTime;

        float coeff;

        if (intoBathroom)
        { 
            // Diminish color temperature and depth of field under water
            while (_depthOfField.focusDistance.value > _depthOfField.focusDistance.min)
            {
                coeff = Time.deltaTime;

                // --- Depth of Field
                _depthOfField.focusDistance.value += dofVal * coeff;

                // --- White Balance 
                _whiteBalance.temperature.value += wbVal * coeff;
                _whiteBalance.tint.value = _whiteBalance.temperature.value;

                yield return null;
            }
        }
        else
        {
            // Reestablish camera settings outside of water
            dofVal *= -1;
            wbVal *= -1;

            while (_depthOfField.focusDistance.value < _DOF_INIT)
            {
                coeff = Time.deltaTime;

                // --- Depth of Field
                _depthOfField.focusDistance.value += dofVal * coeff;

                // --- White Balance 
                _whiteBalance.temperature.value += wbVal * coeff;
                _whiteBalance.tint.value = _whiteBalance.temperature.value;

                yield return null;
            }
        }

        StopTransitionInOutBathroom(intoBathroom);
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// First part of the transition to the ending: make the dancing lights grow bigger and bigger
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToEnding()
    {
        float sizeVal = (_DANCING_LIGHTS_FINAL_SIZE - _dancingLights.startSize) / bedroomTransitionTime;

        while (_dancingLights.startSize < _DANCING_LIGHTS_FINAL_SIZE)
        {
            // Make the dancing lights bigger
            _dancingLights.startSize += sizeVal * Time.deltaTime;
            yield return null;
        }
        EndTransitionToEnding();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Second part of the transition to the ending: make the screen brighter and brighter until all is white
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToEndingPart2()
    {
        // Enable features
        // --- Color Adjustments
        _colorAdjustments.active = true;
        float postExposureVal = (_POST_EXPOSURE_FINAL - _colorAdjustments.postExposure.value) / bedroomTransitionTime;

        while (_colorAdjustments.postExposure.value < _POST_EXPOSURE_FINAL)
        {
            // Make the scene lighter
            _colorAdjustments.postExposure.value += postExposureVal * Time.deltaTime;
            yield return null;
        }

        EndTransitionToEndingPart2(false); // Normal ending, read the letter
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Alternative transition to the ending: make the screen darker and darker until all is black
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToEndingAlternative(bool skipLetter)
    {
        // Everything turns dark if the player refuses to read the letter
        // Only for SI28
        // Enable features
        // --- Color Adjustments
        _colorAdjustments.active = true;        
        _colorAdjustments.colorFilter.overrideState = true;
        _colorAdjustments.colorFilter.value = Color.white;

        _dancingLights.emissionRate = 0;

        float colorVal = -_colorAdjustments.colorFilter.value.r / bedroomTransitionTime;
        float currentColorVal;

        while (_colorAdjustments.colorFilter.value.r > 0f)
        {
            currentColorVal = colorVal * Time.deltaTime;
            _colorAdjustments.colorFilter.value = new Color(_colorAdjustments.colorFilter.value.r + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal, _colorAdjustments.colorFilter.value.b + currentColorVal);

            yield return null;
        }

        EndTransitionToEndingPart2(skipLetter); // Skip all parts with the dancing lights
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Disable color adjustments through time by softening post exposure
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableColorAdjustmentsThroughTime()
    {
        float postExposureVal = -_POST_EXPOSURE_FINAL / creditsTransitionTime;

        while (_colorAdjustments.postExposure.value > 0f)
        {
            // Soften post exposure
            _colorAdjustments.postExposure.value += postExposureVal * Time.deltaTime;
            yield return null;
        }

        EndDisableColorAdjustmentsTransition();
        yield return null;
    }

    // --------------------------------------------------
    // Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start the LaunchSequence transition
    /// </summary>
    public void StartLaunchSequence()
    {
        _launchSequence = StartCoroutine(LaunchSequence());
    }

    // --------------------------------------------------

    /// <summary>
    /// Enable camera effects for the kitchen sequence
    /// </summary>
    public void EnableKitchenCameraEffects()
    {
        // Enable features
        // --- Lens distortion
        _lensDistortion.active = true;

        // --- Film grain
        _filmGrain.active = true;

        // --- Chromatic Aberration
        _chromaticAberration.active = true;

        // --- Channel Mixer
        _channelMixer.active = true;

        // --- Vignette
        _vignette.active = true;

        // Run deformation loop
        _cameraDeformationCoroutine = StartCoroutine(KitchenCameraEffects());
    }

    // --------------------------------------------------

    /// <summary>
    /// Disable camera effects when ending the kitchen sequence
    /// </summary>
    public void DisableKitchenCameraEffects()
    {       
        // Stop deformation loop
        StopCoroutine(_cameraDeformationCoroutine);

        // Disable features
        ResetLensDistortion();
        ResetFilmGrain();
        ResetChromaticAberration();
        ResetChannelMixer();
        ResetVignette();
    }

    // --------------------------------------------------

    /// <summary>
    /// Refactor the camera effect period
    /// </summary>
    public void RefactorKitchenEffectPeriod()
    {
        _kitchenEffectPeriod *= _KITCHEN_EFFECT_COEFF;
    }

    // --------------------------------------------------

    /// <summary>
    /// Get the value of the const [_KITCHEN_EFFECT_COEFF] 
    /// </summary>
    /// <returns>[_KITCHEN_EFFECT_COEFF] value</returns>
    public float GetKitchenEffectCoeff()
    {
        return _KITCHEN_EFFECT_COEFF;
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the transition in and out of the forest sequence
    /// </summary>
    /// <param name="intoForest">true ---> transition to forest ; false ---> transition from forest to reality</param>
    public void StartTransitionInOutForest(bool intoForest)
    {
        // Start transition
        // --- Suddenly all is dark and light starts appearing again (intoForest = true)
        // --- All goes darker and darker then teleport back to reality (intoForest = false)
        _transitionInOutForest = StartCoroutine(TransitionInOutForest(intoForest));
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the transition in and out of the sinking sequence
    /// </summary>
    /// <param name="intoBathroom">true ---> start sinking ; false ---> reached the bottom</param>
    public void StartTransitionInOutBathroom(bool intoBathroom)
    {
        _transitionInOutBathroom = StartCoroutine(TransitionInBathroom(intoBathroom));
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the ending transition
    /// </summary>
    /// <param name="playAlternativeEnding">true ---> play alternative ending where all goes dark ; false ---> play default ending where all goes bright</param>
    /// <param name="skipLetter">true ---> do not display the letter and move directly to credits</param>
    public void StartTransitionToEnding(bool playAlternativeEnding, bool skipLetter)
    {
        // Make the place lighter and lighter until it is pure light (or darker if alternative)
        _transitionToEnding = StartCoroutine(playAlternativeEnding ? TransitionToEndingAlternative(skipLetter) : TransitionToEnding());
    }

    // --------------------------------------------------

    /// <summary>
    /// Assign [_dancingLights] particle system
    /// </summary>
    /// <param name="dancingLights">particle system to use as dancing lights in the bedroom</param>
    public void AssignDancingLights(ParticleSystem dancingLights)
    {
        _dancingLights = dancingLights;
    }

    // --------------------------------------------------

    /// <summary>
    /// Change [_colorAdjustments] saturation value
    /// </summary>
    /// <param name="saturationVal">new saturation value</param>
    public void SetSaturationValue(float saturationVal)
    {
        _colorAdjustments.saturation.value = saturationVal;
    }

    // --------------------------------------------------

    /// <summary>
    /// Enable or disable saturation in [_colorAdjustments]
    /// </summary>
    /// <param name="enable">true ---> set saturation override state active</param>
    public void EnableDisableSaturation(bool enable)
    {
        _colorAdjustments.active = enable;
        _colorAdjustments.saturation.overrideState = enable;
    }

    // --------------------------------------------------

    /// <summary>
    /// Disable color adjustments
    /// </summary>
    /// <param name="instantly">true ---> set inactive ; false ---> use a transition</param>
    public void DisableColorAdjustments(bool instantly)
    {
        if (instantly) _colorAdjustments.active = false;
        else _transitionToEnding = StartCoroutine(DisableColorAdjustmentsThroughTime());
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
// --------------------------------------------------
//  file :      KitchenManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script handling kitchen sequence.
// --------------------------------------------------

using System.Collections;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling transitions and effects in the kitchen sequence
/// </summary>
public class KitchenManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private KitchenObjectClass[] hoveringObjects;
    [SerializeField] private GameObject rainFall = null;
    [SerializeField] private Light[] lights;
    [SerializeField] private GameObject[] tempestOfObjects = null;
    [SerializeField] private float phaseTime = 2.0f;
    [SerializeField] private int nbPhase = 5;
    [SerializeField] private float lightTransitionTime = 3f;

    // private variables
    private GlobalVolumeManager _globalVolumeManager;
    private AudioManager _audioManager; 
    private KnifeAction _knife;
    private float _audioIncRate;
    private float _initLightIntensity;
    private float _tempestRate = 0f;
    private int _nbObjects = 0;

    private Coroutine _increaseMadnessFreqCo = null;
    private Coroutine _lightTransition = null;

    // --------------------------------------------------
    // Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and prepare the sequence by moving objects around and activating specific components on these objects
    /// </summary>
    private void Start()
    {
        // Find managers      
        _globalVolumeManager = transform.parent.Find("RenderingManager").GetComponent<GlobalVolumeManager>();
        _audioManager = transform.parent.Find("AudioManager").GetComponent<AudioManager>();

        _initLightIntensity = lights[0].intensity;

        // Prepare hovering objects for RV01
        if (hoveringObjects.Length > 0)
        {
            _audioIncRate = - 1 / (float) hoveringObjects.Length;

            // Save initial position for all objects
            foreach (var currentObject in hoveringObjects)
            {
                currentObject.oldPos = currentObject.kitchenObject.transform.position;
                currentObject.kitchenObject.GetComponent<Collider>().enabled = false;
                currentObject.kitchenObject.transform.Find("Fireflies").gameObject.SetActive(false);
                currentObject.kitchenObject.GetComponent<Hovering>().enabled = false;
            }
        }

        // Deactivate rain fall at first
        rainFall.SetActive(false);

        // Prepare the tempest of objects for RV01
        if (tempestOfObjects.Length > 0)
        {
            _tempestRate = 2f - _globalVolumeManager.GetKitchenEffectCoeff();
            tempestOfObjects[0].transform.parent.gameObject.SetActive(false);
        }            
    }

    // --------------------------------------------------

    /// <summary>
    /// Move hovering objects to specific positions within the kitchen
    /// </summary>
    private void SetPositionToNewPosition()
    {
        foreach (var currentObject in hoveringObjects)
        {
            currentObject.kitchenObject.transform.position = currentObject.newPos.position;
            currentObject.kitchenObject.GetComponent<Collider>().enabled = true;
            currentObject.kitchenObject.GetComponent<Hovering>().enabled = true;
            currentObject.kitchenObject.transform.Find("Fireflies").gameObject.SetActive(true);
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset all objects' positions upon ending the kitchen sequence
    /// </summary>
    private void ResetPosition()
    {
        foreach (var currentObject in hoveringObjects)
        {
            currentObject.kitchenObject.transform.position = currentObject.oldPos;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the light transition
    /// </summary>
    private void StopLightTransition()
    {
        StopCoroutine(_lightTransition);
    }

    // --------------------------------------------------
    // Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Refactor camera and heart beat effects periodically for SI28
    /// </summary>
    /// <returns></returns>
    private IEnumerator RefactorMadnessFreq()
    {
        _audioIncRate = -(float)(1 / nbPhase);
        for (int i = 0; i < nbPhase; i++)
        {
            // Increase heart beat
            _audioManager.ChangeVolume("SingleHeartBeating", _audioIncRate);

            // Increase heart beat rate visually
            _globalVolumeManager.RefactorKitchenEffectPeriod();

            yield return new WaitForSeconds(phaseTime);
        }
        StopKitchenSeq();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Transition to make the light go brighter or weaker
    /// </summary>
    /// <param name="increaseLight">true ---> increase light intensity</param>
    /// <returns></returns>
    private IEnumerator LightTransition(bool increaseLight)
    {
        // Increase or decrease light intensity
        float lightRate = _initLightIntensity / lightTransitionTime;

        if (increaseLight)
        {
            while (lights[0].intensity < _initLightIntensity)
            {
                foreach (var light in lights) light.intensity += lightRate * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (lights[0].intensity > 0f)
            {
                foreach (var light in lights) light.intensity -= lightRate * Time.deltaTime;
                yield return null;
            }
        }

        StopLightTransition();
        yield return null;
    }

    // --------------------------------------------------
    // Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start the sequence in the kitchen after enabling effects and specific components
    /// </summary>
    public void StartKitchenSeq()
    {
        // Enable Camera effect
        _globalVolumeManager.EnableKitchenCameraEffects();

        // Make the rain fall
        rainFall.SetActive(true);
        _audioManager.Play("Thunderstorm");

        // Show the tempest of objects
        if (tempestOfObjects.Length > 0) tempestOfObjects[0].transform.parent.gameObject.SetActive(true);

        // Move objects
        if (hoveringObjects.Length > 0) SetPositionToNewPosition();
        else
        {
            // Start the coroutine if it is SI28
            _knife.ActivateKnifeAction();
            _increaseMadnessFreqCo = StartCoroutine(RefactorMadnessFreq());
        }

        // Light transition
        _lightTransition = StartCoroutine(LightTransition(false));
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the sequence in the kitchen and reset objects and effects
    /// </summary>
    public void StopKitchenSeq()
    {
        // Disable Camera effect
        _globalVolumeManager.DisableKitchenCameraEffects();

        // Stop audio
        _audioManager.Stop("SingleHeartBeating");
        _audioManager.Stop("Thunderstorm");

        // Stop the rain 
        rainFall.GetComponent<ParticleSystem>().emissionRate = 0;

        // Stop tempest
        if (tempestOfObjects.Length > 0) tempestOfObjects[0].transform.parent.gameObject.SetActive(false);

        // Stop effects
        if (hoveringObjects.Length > 0) ResetPosition(); // Reset object position
        else StopCoroutine(_increaseMadnessFreqCo); // Stop SI28 coroutine

        // Reset knife
        _knife.ResetKnife();

        // Light transition
        _lightTransition = StartCoroutine(LightTransition(true));
    }

    // --------------------------------------------------

    /// <summary>
    /// Refactor madness effect intensity after activating an object in the kitchen
    /// </summary>
    /// <param name="kitchenObject">"mad" firelit object flying around in the kitchen</param>
    public void RefactorMadness(GameObject kitchenObject)
    {
        // Disable Collider and Hovering Script Component
        kitchenObject.GetComponent<Collider>().enabled = false;
        kitchenObject.GetComponent<Hovering>().enabled = false;
        kitchenObject.transform.Find("Fireflies").gameObject.SetActive(false);

        // Increase number of Objects found
        _nbObjects++;
        if (_nbObjects >= hoveringObjects.Length) StopKitchenSeq();
        else
        {
            // Decrease heart beat effect rate
            _audioManager.ChangeVolume("SingleHeartBeating", _audioIncRate);
            _globalVolumeManager.RefactorKitchenEffectPeriod();

            // Decrease tempest speed
            foreach (var tempest in tempestOfObjects) tempest.GetComponent<RollingKnife>().RefactorRotSpeed(_tempestRate);
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Play the sound of a heart beat
    /// </summary>
    public void PlayHeartBeatSound()
    {
        _audioManager.Replay("SingleHeartBeating");
    }

    // --------------------------------------------------

    /// <summary>
    /// Assign a knife action which this manager will activate and deactivate during transition
    /// </summary>
    /// <param name="knife">knife on the table</param>
    public void AssignKnife(KnifeAction knife)
    {
        _knife = knife;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
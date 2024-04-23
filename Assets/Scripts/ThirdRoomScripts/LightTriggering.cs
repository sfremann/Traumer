// --------------------------------------------------
//  file :      LightTriggering.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with light buzzing
//              effect in the corridor.
// --------------------------------------------------

using System.Collections;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling buzzing effect for the lights in the corridor
/// </summary>
public class LightTriggering : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private AudioManager _audioManager;
    private Light _light;
    private float _initIntensity;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and light source
    /// </summary>
    void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _light = transform.Find("Spot Light").gameObject.GetComponent<Light>();
        _initIntensity = _light.intensity;
    }

    // --------------------------------------------------

    /// <summary>
    /// Start a buzzing effect for this lamp when the player pass beneath it
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_light) StartCoroutine(Buzzing());
        if (_audioManager) _audioManager.Play("LightBuzzing");
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Buzzing effect
    /// </summary>
    /// <returns></returns>
    private IEnumerator Buzzing()
    {
        for(int i = 0; i < 2; i++)
        {
            _light.intensity = _initIntensity * 0.5f;
            yield return new WaitForSeconds(0.2f);
            _light.intensity = _initIntensity;
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
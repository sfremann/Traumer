// --------------------------------------------------
//  file :      BedroomManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for handling ending sequence.
// --------------------------------------------------

using System.Collections;
using TMPro;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object used to manage effects and transitions in the bedroom scene
/// </summary>
public class BedroomManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private ParticleSystem dancingLights = null;
    [SerializeField] private GameObject creditsRoom = null;
    [SerializeField] private TextMeshProUGUI credits = null;
    [SerializeField] private GameObject vRCredits = null;
    [SerializeField] private float transitionToCreditsTime = 4f;

    // private variables
    private GlobalVolumeManager _globalVolumeManager;
    private SwitchRenderPipelineAsset _switchRenderPipelineAsset;
    private ChangeScene _changeSceneManager;
    private Material _vRCreditsMat;

    private bool _playAlternativeEnding = false;

    private Coroutine _transitionToCredits;

    // --------------------------------------------------
    // Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        Transform renderingManager = transform.parent.Find("RenderingManager");
        _globalVolumeManager = renderingManager.GetComponent<GlobalVolumeManager>();
        _globalVolumeManager.AssignDancingLights(dancingLights);

        _switchRenderPipelineAsset = renderingManager.GetComponent<SwitchRenderPipelineAsset>();

        _changeSceneManager = transform.parent.Find("ChangingSceneManager").GetComponent<ChangeScene>();

        _vRCreditsMat = vRCredits.GetComponent<Renderer>().material;
        _vRCreditsMat.color = new Color(_vRCreditsMat.color.r, _vRCreditsMat.color.g, _vRCreditsMat.color.b, 0f);
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop transition, it is the end of the experience
    /// </summary>
    private void EndGame()
    {
        StopCoroutine(_transitionToCredits);
        GetComponent<BedroomManager>().enabled = false;
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Make the credits fade in little by little
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayCredits()
    {
        credits.enabled = true;
        credits.color = new Color(credits.color.r, credits.color.g, credits.color.b, 0f);

        float finalColorVal = Color.white.a;
        float colorVal = finalColorVal / transitionToCreditsTime;

        while (credits.color.a < finalColorVal)
        {
            credits.color = new Color(credits.color.r, credits.color.g, credits.color.b, credits.color.a + colorVal * Time.deltaTime);
            yield return null;
        }

        // End the experience
        EndGame();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Make the credits fade in little by little in VR mode
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayVRCredits()
    {
        vRCredits.SetActive(true);        

        float finalColorVal = Color.white.a;
        float colorVal = finalColorVal / transitionToCreditsTime;

        while (_vRCreditsMat.color.a < finalColorVal)
        {
            _vRCreditsMat.color = new Color(_vRCreditsMat.color.r, _vRCreditsMat.color.g, _vRCreditsMat.color.b, _vRCreditsMat.color.a + colorVal * Time.deltaTime);
            yield return null;
        }

        // End the experience
        EndGame();
        yield return null;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start the final sequence of the experience
    /// </summary>
    /// <param name="playAlternativeEnding">true ---> fade to black instead of white</param>
    /// <param name="skipLetter">false ---> do not display the letter</param>
    public void StartEndingScene(bool playAlternativeEnding, bool skipLetter)
    {
        _playAlternativeEnding = playAlternativeEnding;
        _globalVolumeManager.StartTransitionToEnding(_playAlternativeEnding, skipLetter);
    }

    // --------------------------------------------------

    /// <summary>
    /// Start transition to display the credits
    /// </summary>
    public void CreditsRoll()
    {
        if (_transitionToCredits != null) StopCoroutine(_transitionToCredits);

        // Teleport to credits room
        _switchRenderPipelineAsset.UseCreditsRenderSettings();
        _globalVolumeManager.DisableColorAdjustments(false);
        _changeSceneManager.TeleportToScene(dancingLights.transform.parent.gameObject, creditsRoom);

        // Display credits
        _transitionToCredits = StartCoroutine(credits != null ? DisplayCredits() : DisplayVRCredits());
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
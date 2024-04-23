// --------------------------------------------------
//  file :      ColorTheRoom.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       color the room in the picture vision.
// --------------------------------------------------

using UnityEngine;
using System.Collections;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object coloring the room when interacted with
/// </summary>
public class ColorTheRoom : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject secondRing = null;
    [SerializeField] private GameObject holdingHandsSilhouette = null;
    [SerializeField] private float transitionTime = 3f;

    // private variables
    private SaturationController _saturationController;
    private AudioManager _audioManager;

    private Material _silhouetteMat;

    private float _deltaSaturation;

    private Coroutine _transition;

    // const
    private const float _SAT_FINAL_VAL = 1f;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and set silhouette
    /// </summary>
    private void Start()
    {
        GameObject managers = GameObject.Find("Managers");

        _saturationController = managers.transform.Find("RenderingManager").gameObject.GetComponent<SaturationController>();
        
        _audioManager = managers.transform.Find("AudioManager").GetComponent<AudioManager>();

        _deltaSaturation = _SAT_FINAL_VAL / transitionTime;
        _silhouetteMat = holdingHandsSilhouette.GetComponent<Renderer>().materials[0];

        // Hide second ring and silhouette for now
        _silhouetteMat.color = new Color(_silhouetteMat.color.r, _silhouetteMat.color.g, _silhouetteMat.color.b, 0f);
        holdingHandsSilhouette.SetActive(false);
        secondRing.SetActive(false);
    }

    // --------------------------------------------------

    /// <summary>
    /// Start transition
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Show the other ring and the silhouette
        secondRing.SetActive(true);
        holdingHandsSilhouette.SetActive(true);

        // Play a sound
        _audioManager.Play("Laughing");
        _audioManager.Play("Laughing2");

        // Start transition
        _saturationController.StartTransition(true);
        _transition = StartCoroutine(ShowHideHoldingHands(true));
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop transition and hide silhouette if show is false
    /// </summary>
    /// <param name="show">true ---> show ; false ---> hide</param>
    private void StopShowHideHoldingHandsTransition(bool show)
    {
        StopCoroutine(_transition);
        if (show) _transition = StartCoroutine(ShowHideHoldingHands(false));
        else holdingHandsSilhouette.SetActive(false);
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Make a silhouette of holding hands fade in or out
    /// </summary>
    /// <param name="show">true ---> show ; false ---> hide</param>
    /// <returns></returns>
    private IEnumerator ShowHideHoldingHands(bool show)
    {
        float deltaVal = _deltaSaturation;

        if (show)
        {
            while (_silhouetteMat.color.a < _SAT_FINAL_VAL)
            {
                // Let the silhouette fade in slowly  
                _silhouetteMat.color = new Color(_silhouetteMat.color.r, _silhouetteMat.color.g, _silhouetteMat.color.b, _silhouetteMat.color.a + deltaVal * Time.deltaTime);

                yield return null;
            }
        }
        else
        {
            deltaVal *= (-0.425f);

            while (_silhouetteMat.color.a > 0f)
            {
                // Let the silhouette fade out slowly  
                _silhouetteMat.color = new Color(_silhouetteMat.color.r, _silhouetteMat.color.g, _silhouetteMat.color.b, _silhouetteMat.color.a + deltaVal * Time.deltaTime);

                yield return null;
            }
        }     

        StopShowHideHoldingHandsTransition(show);
        yield return null;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
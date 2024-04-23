// --------------------------------------------------
//  file :      UnColorTheRoom.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       uncolor the room in the picture
//              vision.
// --------------------------------------------------

using UnityEngine;
using System.Collections;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object uncoloring the room when interacted with
/// </summary>
public class UnColorTheRoom : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject singleHandSilhouette = null;
    [SerializeField] private float transitionTime = 3f;

    // private variables
    private SaturationController _saturationController;

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
        _saturationController = GameObject.Find("Managers").transform.Find("RenderingManager").gameObject.GetComponent<SaturationController>();


        _deltaSaturation = _SAT_FINAL_VAL / transitionTime;
        _silhouetteMat = singleHandSilhouette.GetComponent<Renderer>().materials[0];

        // Hide second silhouette for now
        _silhouetteMat.color = new Color(_silhouetteMat.color.r, _silhouetteMat.color.g, _silhouetteMat.color.b, 0f);
        singleHandSilhouette.SetActive(false);
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

        // Show the silhouette
        singleHandSilhouette.SetActive(true);

        // Start transition
        _transition = StartCoroutine(ShowHideSingleHand(true));
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop transition and hide silhouette if show is false
    /// </summary>
    /// <param name="show">true ---> show ; false ---> hide</param>
    private void StopShowHideSingleHandTransition(bool show)
    {
        StopCoroutine(_transition);
        if (show) _transition = StartCoroutine(ShowHideSingleHand(false));
        else _saturationController.StartTransition(false); // Leave the picture vision
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Make a silhouette of a single hand fade in or out
    /// </summary>
    /// <param name="show">true ---> show ; false ---> hide</param>
    /// <returns></returns>
    private IEnumerator ShowHideSingleHand(bool show)
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

        StopShowHideSingleHandTransition(show);
        yield return null;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
// --------------------------------------------------
//  file :      KnifeAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the knife for
//              the kitchen scene.
// --------------------------------------------------

using System.Collections;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the knife's behavior in the kitchen sequence
/// </summary>
public class KnifeAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private Camera _playerCamera;
    private KitchenManager _kitchenManager;
    private ChangingRoom _changingRoomManager;
    private Coroutine _facePlayer;
    private Quaternion _initRot;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and get player camera
    /// </summary>
    private void Start()
    {
        GameObject managers = GameObject.Find("Managers");

        _kitchenManager = managers.transform.Find("KitchenManager").GetComponent<KitchenManager>();
        _kitchenManager.AssignKnife(GetComponent<KnifeAction>());

        _changingRoomManager = managers.transform.Find("ChangingRoomManager").GetComponent<ChangingRoom>();

        _playerCamera = managers.transform.Find("RV_GameManager").GetComponent<RV_GameManager>().IsVR() ? GameObject.Find("Player").GetComponentInChildren<Camera>() : GameObject.Find("FirstPersonController").GetComponentInChildren<Camera>();
        
        _initRot = transform.rotation;
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the kitchen sequence
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        ActivateKnifeAction();

        // Start kitchen sequence
        _kitchenManager.StartKitchenSeq();
    }
    
    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Make the knife point towards the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator FacePlayer()
    {
        while (true)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_playerCamera.transform.position - transform.position, Vector3.right);
            transform.rotation = Quaternion.Euler(0f, (targetRotation.eulerAngles.y - 90.0f), 0f);

            yield return null;
        }
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Reset the knife to its initial position and remove KnifeAction component
    /// </summary>
    public void ResetKnife()
    {
        // Reset knife and open the next door
        StopCoroutine(_facePlayer);
        transform.rotation = _initRot;
        _changingRoomManager.ChangeRoom();
        GetComponent<KnifeAction>().enabled = false;
    }

    // --------------------------------------------------

    /// <summary>
    /// Start making the knife face the player
    /// </summary>
    public void ActivateKnifeAction()
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);
        _facePlayer = StartCoroutine(FacePlayer());
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
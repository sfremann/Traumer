// --------------------------------------------------
//  file :      GuidePlayer.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       10/11/23
//  desc:       script used to manage the guide in the 
//              forest scene, makes the guide follow
//              a defined trajectory or automatically 
//              follow a waypoint.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the guide's behavior in the forest, making is follow a defined trajectory
/// </summary>
public class GuidePlayer : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized attributes
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform[] decisionWaypoints;
    [SerializeField] private float speed          = 1.0f;
    [SerializeField] private float maxSpeed       = 3.0f;
    [SerializeField] private float distFollowing  = 6.0f;
    [SerializeField] private float distFollower   = 2.0f;

    // private attributes
    private float _speed        = 0.0f;
    private int _currPoint      = 0;
    private int _currDecision   = 0;
    private bool _followPlayer  = false;
    private Transform _player   = null;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Set starting speed
    /// </summary>
    private void Start()
    {
        _speed = speed;
    }

    // --------------------------------------------------

    /// <summary>
    /// Change the guide's path depending on the player's actions
    /// </summary>
    private void Update()
    {
        if(_player && _followPlayer) // The player is followed by the guide
        {
            if (Vector3.Distance(transform.position, _player.position) > distFollower)
            {
                Vector3 direction = _player.position - transform.position;
                transform.Translate(speed * Time.deltaTime * direction.normalized);
            }
        }
        else // The player follows the guide (OR the guide is in automatic mode)
        {
            if ( _player && (Vector3.Distance(transform.position, _player.position) < distFollowing) || (_speed == maxSpeed) )
            {
                // Follow the player
                if(_currPoint < waypoints.Length)
                {
                    Vector3 direction = waypoints[_currPoint].position - transform.position;
                    transform.Translate(speed * Time.deltaTime * direction.normalized);

                    if (Vector3.Distance(transform.position, waypoints[_currPoint].position) < 0.3f)
                    {
                        if(_speed == maxSpeed) _speed = speed;
                        _currPoint++;
                    }
                }
            }
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Activate the guide when the player is close enough, it will follow its path through the forest
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_player == null) _player = other.transform;
        GetComponent<Collider>().enabled = false;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Change the guide's behavior, it follows the player if it did not before or it stops following the player
    /// </summary>
    public void ChangeMode()
    {   
        // Change mode
        _followPlayer = !_followPlayer;

        if ((!_followPlayer) && _currDecision < decisionWaypoints.Length)
        {
            // Update the current point
            int index = System.Array.IndexOf(waypoints, decisionWaypoints[_currDecision]);
            _currPoint = (index == -1) ? _currPoint : index;

            // Change speed to return to next waypoints
            _speed = maxSpeed;

            // Check for next decision
            _currDecision++;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Increase the number of times the player encountered a crossing and had to make a choice between following the guide and go their own way
    /// </summary>
    public void ChoseGuide()
    {
        _currDecision++;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
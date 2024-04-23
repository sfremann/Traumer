// --------------------------------------------------
//  file :      ChangingRoom.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the change of
//              rooms.
// --------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling transitions between rooms
/// </summary>
public class ChangingRoom : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject[] rooms = null;
    [SerializeField] private float doorRotTime = 1.5f;

    // enumerations
    private enum CurrentRoomStatus
    {
        _IN_CURRENT_ROOM,
        _ENTERING_NEXT_ROOM
    };

    // private variables
    private CurrentRoomStatus _status = CurrentRoomStatus._IN_CURRENT_ROOM;
    private int _currentRoomNumber = 0;
    private GameObject _currentRoom;
    private GameObject _nextRoom;
    private GameObject _doorIn;
    private GameObject _connectionWall;

    private AudioManager _audioManager;
    private bool _destroyRoom = true;

    private readonly List<Coroutine> _coroutines = new();

    /*
     * DoorIn and DoorOut are two GameObjects refering to the same door
     * DoorOut is the door of the _currentRoom used to exit the room
     * DoorIn is the same door seen from the perspective of the _nextRoom
     * ConnectionWall is the connection wall between the rooms seen from the _nextRoom
     */

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find audio manager and assign first rooms
    /// </summary>
    private void Start()
    {
        _audioManager = transform.parent.Find("AudioManager").GetComponent<AudioManager>();
        if (rooms.Length == 0) Debug.LogError("Please, provide an array of rooms.");
        else
        {
            UpdateCurrentRoom();
            GetRoomSound();
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Update current room based on [_currentRoomNumber]
    /// </summary>
    private void UpdateCurrentRoom()
    {
        _currentRoom = rooms[_currentRoomNumber];
        if (_currentRoomNumber < rooms.Length - 1)
        {
            _nextRoom = rooms[(_currentRoomNumber + 1) % rooms.Length];
            _doorIn = _nextRoom.transform.Find("DoorIn").gameObject;
            _connectionWall = _nextRoom.transform.Find("Walls").Find("ConnectionWall").gameObject;
        }
        else // The player is in the last room
        {
            _nextRoom = null;
            _doorIn = null;
            _connectionWall = null;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Open the door and show the next room
    /// </summary>
    private void OpenDoor()
    {
        GameObject doorOut = _currentRoom.transform.Find("DoorOut").gameObject;
        doorOut.GetComponent<Collider>().enabled = false;
        _coroutines.Add(StartCoroutine(OpenThisDoor(doorOut.transform.Find("doorWing"))));

        _nextRoom.SetActive(true);
        _connectionWall.SetActive(false);
        _doorIn.SetActive(false);

        if(_audioManager) _audioManager.Play("OpenDoor");
    }

    // --------------------------------------------------

    /// <summary>
    /// Close the door, hide the previous room and turn on the wall
    /// </summary>
    private void CloseDoor()
    {
        _currentRoom.SetActive(false);
        if (_destroyRoom) Destroy(_currentRoom);

        _connectionWall.SetActive(true);
        _doorIn.SetActive(true);

        if (_audioManager) _audioManager.Play("CloseDoor");
    }

    // --------------------------------------------------

    /// <summary>
    /// Play the right sound depending on the [_currentRoomNumber]
    /// </summary>
    private void GetRoomSound()
    {
        switch(_currentRoomNumber)
        {
            case 0:
            case 1:
                _audioManager.Replay("WhiteNoise");
                break;
            case 2:
                _audioManager.Stop("WhiteNoise");
                _audioManager.Replay("LightSound");
                break;
            case 3:
                _audioManager.Stop("PhoneRinging"); // In case is still ringing
                break;
            default:
                break;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop oldest door transition
    /// </summary>
    private void StopOldestCoroutine()
    {
        StopCoroutine(_coroutines[0]);
        _coroutines.RemoveAt(0);
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Play animation of door opening
    /// </summary>
    /// <param name="thisDoorWing">door wing to rotate</param>
    /// <returns></returns>
    private IEnumerator OpenThisDoor(Transform thisDoorWing)
    {
        float rotVal = 90f / doorRotTime;
        float timeLimit = Time.timeSinceLevelLoad + doorRotTime;

        while (Time.timeSinceLevelLoad < timeLimit)
        {
            thisDoorWing.Rotate(Vector3.up, rotVal * Time.deltaTime);            
            yield return null;
        }

        StopOldestCoroutine();
        yield return null;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Move forward in the appartment
    /// </summary>
    /// <param name="destroyRoom">true ---> destroy room object</param>
    public void ChangeRoom(bool destroyRoom = true)
    {
        // Store the choice to destroy the room or not
        _destroyRoom = destroyRoom;

        // Change the room according to the state
        if (_status == CurrentRoomStatus._IN_CURRENT_ROOM)
        {
            _status = CurrentRoomStatus._ENTERING_NEXT_ROOM;
            OpenDoor();
        }
        else if (_status == CurrentRoomStatus._ENTERING_NEXT_ROOM)
        {
            _currentRoomNumber++;
            _status = CurrentRoomStatus._IN_CURRENT_ROOM;
            CloseDoor();
            UpdateCurrentRoom();
            GetRoomSound();
        }
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
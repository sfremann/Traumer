// --------------------------------------------------
//  file :      KitchenObjectClass.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       KitchenObjectClass class definition
//              regrouping object/position/starting
//              position for the kitchen sequence.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object used for handling hovering objects in the kitchen sequence
/// </summary>
[System.Serializable]
public class KitchenObjectClass
{
    public GameObject kitchenObject;
    public Transform newPos;
    [HideInInspector]
    public Vector3 oldPos;
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
// --------------------------------------------------
//  file :      Sound.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       Sound class definition regrouping
//              interesting sound parameters.
// --------------------------------------------------

using UnityEngine;

/// <summary>
/// Object regrouping interesting sound parameters
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
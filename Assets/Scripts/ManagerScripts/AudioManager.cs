// --------------------------------------------------
//  file :      AudioManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script used for audio management.
// --------------------------------------------------

using System.Collections;
using UnityEngine;
using System;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object used for audio management
/// </summary>
public class AudioManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private Sound[] sounds;
    [SerializeField] private float defaultTransitionTime = 0.2f; // factor used for tweaking transition duration

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find clip and set AudioSource for every sound of the list [sounds]
    /// </summary>
    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }

        // Play breath sounds in the background
        Play("Breathing");
    }

    // --------------------------------------------------

    /// <summary>
    /// Find a sound [name] in the list based on its name
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    /// <returns></returns>
    private Sound FindSound(string name)
    {
        return Array.Find(sounds, s => s.name == name);
    }

    // --------------------------------------------------
    //  Coroutine
    // --------------------------------------------------

    /// <summary>
    /// Set the volume of a sound [s] to a new value [wantedVolume] through a period of time [transitionTime]
    /// </summary>
    /// <param name="s">sound in the list</param>
    /// <param name="transitionTime">transition duration, [transitionTime] should be multiple of [defaultTransitionTime]</param>
    /// <param name="stopSound">true ---> reduce sound volume until it cannot be heard anymore ; false ---> start playing the sound at a low level and increase the volume</param>
    /// <param name="wantedVolume">final volume value</param>
    /// <returns></returns>
    private IEnumerator TransitionSoundSlowly(Sound s, float transitionTime, bool stopSound, float wantedVolume)
    {
        int nbOfState = (int)(transitionTime / defaultTransitionTime);
        float volumeValue = ((stopSound ? s.source.volume : wantedVolume) / nbOfState);

        for (int i = 0; i < nbOfState; i++)
        {
            // Increase or decrease volume until end of transition
            if (stopSound && s.source.isPlaying) s.source.volume -= volumeValue;
            else if(!stopSound)
            {
                if(!s.source.isPlaying)
                {
                    s.source.Play();
                    s.source.volume = 0.01f;
                }
                s.source.volume += volumeValue;
            }

            yield return new WaitForSeconds(defaultTransitionTime);
        }
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Replay a sound [name] from beginning
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    public void Replay(string name)
    {
        Sound s = FindSound(name);
        if (s != null) 
        {
            if (s.source.isPlaying) s.source.Stop();
            s.source.Play();
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Play a sound [name]
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    public void Play(string name)
    {
        Sound s = FindSound(name);
        if ((s != null) && (!s.source.isPlaying))s.source.Play();
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop a sound [name]
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    public void Stop(string name)
    {
        Sound s = FindSound(name);
        if ((s != null) && (s.source.isPlaying))s.source.Stop();
    }

    // --------------------------------------------------

    /// <summary>
    /// Pause a sound [name]
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    public void Pause(string name)
    {
        Sound s = FindSound(name);
        if ((s != null) && (s.source.isPlaying)) s.source.Pause();
    }

    // --------------------------------------------------

    /// <summary>
    /// Add [volumeOffset] value to the volume of a sound [name]
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    /// <param name="volumeOffset">value to add to the volume</param>
    public void ChangeVolume(string name, float volumeOffset)
    {
        Sound s = FindSound(name);
        if ((s != null) && (s.source.isPlaying))
        {
            if (s.source.volume + volumeOffset > 1.0f) s.source.volume = 1.0f;
            else
            {
                if (s.source.volume + volumeOffset < 0.0f) s.source.volume = 0.0f;
                else s.source.volume += volumeOffset;
            }
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Start the sound transition for a sound [name], the transition will for [transitionTime] seconds
    /// </summary>
    /// <param name="name">name of the sound in the list</param>
    /// <param name="transitionTime">transition duration, [transitionTime] should be multiple of [defaultTransitionTime]</param>
    /// <param name="stopSound">true ---> reduce sound volume until it cannot be heard anymore ; false ---> start playing the sound at a low level and increase the volume</param>
    /// <param name="wantedVolume">final volume value</param>
    public void SoundTransition(string name, float transitionTime, bool stopSound = true, float wantedVolume = 0.1f)
    {
        Sound s = FindSound(name);
        if (s != null) StartCoroutine(TransitionSoundSlowly(s, transitionTime, stopSound, wantedVolume));
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
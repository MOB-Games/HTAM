using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip audioClip;
    [Range(0f,1f)]
    public float volume;
    [Range(-3f,3f)]
    public float pitch;
    public bool loop;

    [HideInInspector] 
    public AudioSource source;

    [HideInInspector]
    public bool hasSource;

    public void Play()
    {
        if (hasSource)
            source.Play();
    }

    public void Stop()
    {
        if (hasSource)
            source.Stop();
    }
}

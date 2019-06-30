using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float spatialBlend;
    public bool loop = true;

    [HideInInspector]
    public AudioSource source;

    
}



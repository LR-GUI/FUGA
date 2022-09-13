using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    [Range(0f,1f)]
    public float volume;

    [Range(0,30f)]
    public float pitch;

    //public float high = 0;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
}

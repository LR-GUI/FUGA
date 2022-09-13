using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    public Sound[] sounds;

    public static SoundManager Instance;
    // Start is called before the first frame update
    void Awake()
    {  

        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.source.pitch + s.pitch;
            s.source.loop = s.loop;

        }
        
    }


    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s==null) {return;}
        //StartCoroutine(FadeOut());
        s.source.Play();
        //StartCoroutine(FadeIn());
    }

    public void Stop() {
        foreach (Sound s in sounds)
        {
            if(s.source.isPlaying) {
                s.source.Stop();
            }
            
        }
    }

}
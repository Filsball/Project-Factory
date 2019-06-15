using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    AudioSource dunkelheit;
    AudioSource hintergrund;
    AudioSource saferoom;
    AudioSource generatorStartend;
    public static bool keepFadingIn;
    public static bool keepFadingOut;
    public static AudioManager instance;
    [HideInInspector]
    public bool generatorStarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }
    
    void Start()
    {
        hintergrund = Array.Find(sounds, sound => sound.name == "Hintergrund(1)").source;
        dunkelheit = Array.Find(sounds, sound => sound.name == "InDunkelheit(2)").source;
        saferoom = Array.Find(sounds, sound => sound.name == "Saferoom(1)").source;
        generatorStartend = Array.Find(sounds, sound => sound.name == "GeneratorStartend").source;
        Play("Hintergrund(1)", 0.7f);
    }

    

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown("b")) // Placeholder fuer event: 5 Sekunden in Dunkelheit
        {
            
            if (hintergrund.volume > 0.01)
            {
                instance.StopAllCoroutines();
                FadeCallerMitPP(dunkelheit, hintergrund, 0.9f, 3f, true);
            }


        }
        if (Input.GetKeyDown("n")) // Placeholder für event: in Saferoom Hitbox
        {
            if (saferoom.volume > 0.01)
            {
                if(hintergrund.volume > 0.01)
                {
                    if (dunkelheit.volume > 0.01)
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, hintergrund, dunkelheit, 0.7f, 3f, false);
                    }
                    else
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, hintergrund, 0.7f, 3f, false);
                    }
                }
                else
                {
                    if (dunkelheit.volume > 0.01)
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, dunkelheit, hintergrund, 0.7f, 3f, false);
                    }
                    else
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, hintergrund, 0.7f, 3f, false);
                    }   
                }
            }
            else
            {
                if (hintergrund.volume > 0.01)
                {
                    if (dunkelheit.volume > 0.01)
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, hintergrund, dunkelheit, 0.7f, 3f, false);
                    }
                    else
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, hintergrund, 0.7f, 3f, false);
                    }
                }
                else
                {
                    if (dunkelheit.volume > 0.01)
                    {
                        StopAllCoroutines();
                        FadeCaller(saferoom, dunkelheit, 0.7f, 3f, false);
                    }
                    else
                    {
                        StopAllCoroutines();
                        FadeInCaller(saferoom, 0.7f, 3f, false);
                    }
                }
            }
        }

        if (Input.GetKeyDown("m")) { //Placeholder fuer event: In normaler Licht Hitbox
            if(dunkelheit.volume > 0.01)
            {
                instance.StopAllCoroutines();
                FadeCaller(hintergrund, dunkelheit, 0.7f, 3f, false);
            }
            else if(saferoom.volume > 0.01)
            {
                instance.StopAllCoroutines();
                FadeCaller(hintergrund, saferoom, 0.7f, 3f, false);
            }
            else
            {
                instance.StopAllCoroutines();
                FadeInCaller(hintergrund, 0.7f, 1f, false);
            }
        }
        if (generatorStarted && !generatorStartend.isPlaying)
        {
            generatorStarted = false;
            Play("GeneratorLaufend", 1.0f, generatorStartend.transform.position);
        }
    }
    

    public static void FadeInCaller (AudioSource toFadeIn, float maxVolume, float time, Boolean startNew)
    {
        instance.StartCoroutine(fadeIn(toFadeIn, maxVolume, time, startNew));
    }

    public static void FadeOutCaller(AudioSource toFadeOut, float time)
    {
        instance.StartCoroutine(fadeOut(toFadeOut, time));
    }

    public static void FadeCaller(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew)
    {
        instance.StartCoroutine(fadeSounds(toFadeIn, toFadeOut, maxVolume, time, startNew));
    }
    public static void FadeCallerMitPP(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew)
    {
        instance.StartCoroutine(fadeSoundsMitPP(toFadeIn, toFadeOut, maxVolume, time, startNew));
    }

    public static void FadeCaller(AudioSource toFadeIn, AudioSource toFadeOut, AudioSource toFadeOut2, float maxVolume, float time, Boolean startNew)
    {
        instance.StartCoroutine(fadeSounds(toFadeIn, toFadeOut, toFadeOut2, maxVolume, time, startNew));
    }

    static IEnumerator fadeIn(AudioSource toFadeIn, float maxVolume, float time, Boolean startNew)
    {
        keepFadingIn = true;
        keepFadingOut = false;
        if (startNew) toFadeIn.Play();

        while(toFadeIn.volume < maxVolume && keepFadingIn)
        {
            toFadeIn.volume += Time.deltaTime / time;
            yield return null;
        }
    }

    static IEnumerator fadeOut(AudioSource toFadeOut, float time)
    {
        keepFadingIn = false;
        keepFadingOut = true;
        float startVolume = toFadeOut.volume;

        while (toFadeOut.volume > 0 && keepFadingOut)
        {
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            
            yield return null;
        }
        toFadeOut.Stop();

    }

    static IEnumerator fadeSounds(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew)
    {
        if (startNew || !toFadeIn.isPlaying) toFadeIn.Play();
        float startVolume = toFadeOut.volume;

        while (toFadeIn.volume < maxVolume || toFadeOut.volume > 0)
        {
            if(toFadeIn.volume < maxVolume) toFadeIn.volume += Time.deltaTime / time;
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            yield return null;
        }
    }

    static IEnumerator fadeSoundsMitPP(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew)
    {
        if (startNew || !toFadeIn.isPlaying) toFadeIn.Play();
        float startVolume = toFadeOut.volume;

        while (toFadeIn.volume < maxVolume || toFadeOut.volume > 0)
        {
            if (toFadeIn.volume < maxVolume) toFadeIn.volume += Time.deltaTime / time;
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            yield return null;
        }
    }

    static IEnumerator fadeSounds(AudioSource toFadeIn, AudioSource toFadeOut, AudioSource toFadeOut2, float maxVolume, float time, Boolean startNew)
    {
        if (startNew || !toFadeIn.isPlaying) toFadeIn.Play();
        float startVolume = toFadeOut.volume;
        float startVolume2 = toFadeOut2.volume;

        while (toFadeIn.volume < maxVolume || toFadeOut.volume > 0)
        {
            if (toFadeIn.volume < maxVolume) toFadeIn.volume += Time.deltaTime / time;
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            toFadeOut2.volume -= startVolume2 * Time.deltaTime / time;
            yield return null;
        }
    }

    public void ClipCaller(String startend, String laufend, float volume, Vector3 position)
    {
        instance.StartCoroutine(ClipMitUebergang(startend, laufend, volume, position));
    }

    public IEnumerator ClipMitUebergang(String startend, String laufend, float volume, Vector3 position)
    {
        float time = getTime(startend);
        float counter = 10.0f;

        Play(startend, volume, position);

        while (time > 0)
        {
            counter -= 10.0f * Time.deltaTime / time;
            yield return null;
        }
        Play(laufend, volume, position);
    }

    

    public void Play(string name, float volume, Vector3 position)
    {
        AudioSource s = Array.Find(sounds, sound => sound.name == name).source;
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.transform.position = position;
        s.volume = volume;
        s.Play();
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = 0.7f;
        s.source.Play();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public float getTime(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.clip.length;
    }

    public void setPosition(string name, Vector3 position)
    {
        AudioSource s = Array.Find(sounds, sound => sound.name == name).source;
        s.transform.position = position;
    }

    static IEnumerator postProcess()
    {
        yield return null;
    }
}

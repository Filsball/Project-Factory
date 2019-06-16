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
    public PostProcessingBehaviour pPB;

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
        pPB = FindObjectOfType<PostProcessingBehaviour>();
        MotionBlurModel.Settings MBS = pPB.profile.motionBlur.settings;
        MBS.frameBlending = 0;
        ColorGradingModel.Settings CGS = pPB.profile.colorGrading.settings;
        CGS.basic.contrast = 1;
        CGS.basic.saturation = 1;
        VignetteModel.Settings VS = pPB.profile.vignette.settings;
        VS.opacity = 0;
        ChromaticAberrationModel.Settings CAS = pPB.profile.chromaticAberration.settings;
        CAS.intensity = 0;
        pPB.profile.motionBlur.settings = MBS;
        pPB.profile.colorGrading.settings = CGS;
        pPB.profile.vignette.settings = VS;
        pPB.profile.chromaticAberration.settings = CAS;



        hintergrund = Array.Find(sounds, sound => sound.name == "Hintergrund").source;
        dunkelheit = Array.Find(sounds, sound => sound.name == "InDunkelheit").source;
        saferoom = Array.Find(sounds, sound => sound.name == "Saferoom").source;
        generatorStartend = Array.Find(sounds, sound => sound.name == "GeneratorStartend").source;
        Play("Hintergrund", 0.7f);
    }

    

    // Update is called once per frame
    public void Update()
    {
        
        if (Input.GetKeyDown("b")) // Placeholder fuer event: 5 Sekunden in Dunkelheit
        {
            
            if (hintergrund.volume > 0.01)
            {
                instance.StopAllCoroutines();
                FadeCallerMitPP(dunkelheit, hintergrund, 0.9f, 3f, true, pPB);
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
                        FadeCallerMitPPBackwards(saferoom, hintergrund, dunkelheit, 0.7f, 3f, false, pPB);
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
                        FadeCallerMitPPBackwards(saferoom, dunkelheit, hintergrund, 0.7f, 3f, false, pPB);
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
                        FadeCallerMitPPBackwards(saferoom, hintergrund, dunkelheit, 0.7f, 3f, false, pPB);
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
                        FadeCallerMitPPBackwards(saferoom, dunkelheit, 0.7f, 1.5f, false, pPB);
                        Play("Atmung", 0.2f);
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
                FadeCallerMitPPBackwards(hintergrund, dunkelheit, 0.7f, 3f, false, pPB);
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
    public static void FadeCaller(AudioSource toFadeIn, AudioSource toFadeOut, AudioSource toFadeOut2, float maxVolume, float time, Boolean startNew)
    {
        instance.StartCoroutine(fadeSounds(toFadeIn, toFadeOut, toFadeOut2, maxVolume, time, startNew));
    }
    public static void FadeCallerMitPP(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew, PostProcessingBehaviour pPB)
    {
        instance.StartCoroutine(fadeSoundsMitPP(toFadeIn, toFadeOut, maxVolume, time, startNew, pPB));
    }
    public static void FadeCallerMitPPBackwards(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew, PostProcessingBehaviour pPB)
    {
        instance.StartCoroutine(fadeSoundsMitPPBackwards(toFadeIn, toFadeOut, maxVolume, time, startNew, pPB));
    }
    public static void FadeCallerMitPPBackwards(AudioSource toFadeIn, AudioSource toFadeOut, AudioSource toFadeOut2, float maxVolume, float time, Boolean startNew, PostProcessingBehaviour pPB)
    {
        instance.StartCoroutine(fadeSoundsMitPPBackwards( toFadeIn,  toFadeOut,  toFadeOut2,  maxVolume,  time,  startNew,  pPB));
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
    static IEnumerator fadeSoundsMitPP(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew, PostProcessingBehaviour pPB)
    {
        if (startNew || !toFadeIn.isPlaying) toFadeIn.Play();
        float startVolume = toFadeOut.volume;
        PostProcessingProfile profile = pPB.profile;
        float pufferzeit = 10f;
        float wartezeit = 10f;
        float wartezeitStartwert = wartezeit;
        float PPFadeZeit = 20f;
       

        while (toFadeIn.volume < maxVolume || toFadeOut.volume > 0)
        {
            if (toFadeIn.volume < maxVolume) toFadeIn.volume += Time.deltaTime / time;
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            yield return null;
        }
        while (pufferzeit > 0)
        {
            pufferzeit -= wartezeitStartwert * Time.deltaTime / wartezeit;
            yield return null;
        }
        while (profile.motionBlur.settings.frameBlending < 1)
        {
            PPMotionBlur(pPB, profile, PPFadeZeit, true);
            PPColorGrading(pPB, profile, PPFadeZeit, true);
            PPVignette(pPB, profile, PPFadeZeit, true);
            PPChromaticAberration(pPB, profile, PPFadeZeit, true);
            yield return null;
        }
    }
    static IEnumerator fadeSoundsMitPPBackwards(AudioSource toFadeIn, AudioSource toFadeOut, float maxVolume, float time, Boolean startNew, PostProcessingBehaviour pPB)
    {
        if (startNew || !toFadeIn.isPlaying) toFadeIn.Play();
        float startVolume = toFadeOut.volume;
        PostProcessingProfile profile = pPB.profile;
        float PPFadeZeit = 1.5f;


        while (toFadeIn.volume < maxVolume || toFadeOut.volume > 0 || profile.motionBlur.settings.frameBlending > 0.05)
        {
            if (toFadeIn.volume < maxVolume) toFadeIn.volume += Time.deltaTime / time;
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            PPMotionBlur(pPB, profile, PPFadeZeit, false);
            PPColorGrading(pPB, profile, PPFadeZeit, false);
            PPVignette(pPB, profile, PPFadeZeit, false);
            PPChromaticAberration(pPB, profile, PPFadeZeit, false);
            yield return null;

        }
        MotionBlurModel.Settings moBlurSettings = profile.motionBlur.settings;
        moBlurSettings.frameBlending = 0;
        profile.motionBlur.settings = moBlurSettings;
        ChromaticAberrationModel.Settings chromaticAberrationSettings = profile.chromaticAberration.settings;
        chromaticAberrationSettings.intensity = 0;
        profile.chromaticAberration.settings = chromaticAberrationSettings;
        VignetteModel.Settings vignetteSettings = profile.vignette.settings;
        vignetteSettings.opacity = 0;
        profile.vignette.settings = vignetteSettings;

    }
    static IEnumerator fadeSoundsMitPPBackwards(AudioSource toFadeIn, AudioSource toFadeOut, AudioSource toFadeOut2, float maxVolume, float time, Boolean startNew, PostProcessingBehaviour pPB)
    {
        if (startNew || !toFadeIn.isPlaying) toFadeIn.Play();
        float startVolume = toFadeOut.volume;
        float startVolume2 = toFadeOut2.volume;
        PostProcessingProfile profile = pPB.profile;
        float PPFadeZeit = 1.5f;


        while (toFadeIn.volume < maxVolume || toFadeOut.volume > 0 || profile.motionBlur.settings.frameBlending > 0.05)
        {
            if (toFadeIn.volume < maxVolume) toFadeIn.volume += Time.deltaTime / time;
            toFadeOut.volume -= startVolume * Time.deltaTime / time;
            toFadeOut2.volume -= startVolume2 * Time.deltaTime / time;
            PPMotionBlur(pPB, profile, PPFadeZeit, false);
            PPColorGrading(pPB, profile, PPFadeZeit, false);
            PPVignette(pPB, profile, PPFadeZeit, false);
            PPChromaticAberration(pPB, profile, PPFadeZeit, false);
            yield return null;
        }
        MotionBlurModel.Settings moBlurSettings = profile.motionBlur.settings;
        moBlurSettings.frameBlending = 0;
        profile.motionBlur.settings = moBlurSettings;
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
        s.source.volume = volume;
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
    public static void PPMotionBlur(PostProcessingBehaviour pPB, PostProcessingProfile profile, float time, Boolean forward)
    {
        MotionBlurModel.Settings moBlurSettings = profile.motionBlur.settings;
        float startwert = moBlurSettings.frameBlending;
        if (forward)
        {
            moBlurSettings.frameBlending += Time.deltaTime / time;
            profile.motionBlur.settings = moBlurSettings;
        }
        else
        {
            moBlurSettings.frameBlending -= startwert * Time.deltaTime / time;
            profile.motionBlur.settings = moBlurSettings;
        }
            
    }
    public static void PPColorGrading(PostProcessingBehaviour pPB, PostProcessingProfile profile, float time, Boolean forward)
    {
        ColorGradingModel.Settings colorGradingSettings = profile.colorGrading.settings;
        float saturationStartwert = colorGradingSettings.basic.saturation;
        float contrastStartwert = colorGradingSettings.basic.contrast;
        if (forward)
        {
            if(colorGradingSettings.basic.contrast < 1.7f)
            {
                colorGradingSettings.basic.contrast += Time.deltaTime / time;
            }
            if (colorGradingSettings.basic.saturation > 0.5f)
            {
                colorGradingSettings.basic.saturation -= saturationStartwert * Time.deltaTime / time;
            }
            profile.colorGrading.settings = colorGradingSettings;
        }
        else
        {
            if(colorGradingSettings.basic.contrast > 1f)
            {
                colorGradingSettings.basic.contrast -= contrastStartwert * Time.deltaTime / time;
            }
            if(colorGradingSettings.basic.saturation < 1f)
            {
                colorGradingSettings.basic.saturation += Time.deltaTime / time;
            }
            profile.colorGrading.settings = colorGradingSettings;
        }
        
    }
    public static void PPVignette(PostProcessingBehaviour pPB, PostProcessingProfile profile, float time, Boolean forward)
    {
        VignetteModel.Settings vignetteSettings = profile.vignette.settings;
        float startwert = vignetteSettings.opacity;
        if (forward)
        {
            vignetteSettings.opacity += Time.deltaTime / time;
            profile.vignette.settings = vignetteSettings;
        }
        else
        {
            vignetteSettings.opacity -= startwert * Time.deltaTime / time;
            profile.vignette.settings = vignetteSettings;
        }
        
    }
    public static void PPChromaticAberration(PostProcessingBehaviour pPB, PostProcessingProfile profile, float time, Boolean forward)
    {
        ChromaticAberrationModel.Settings chromaticAberrationSettings = profile.chromaticAberration.settings;
        float startwert = chromaticAberrationSettings.intensity;
        if (forward)
        {
            chromaticAberrationSettings.intensity += Time.deltaTime / time;
            profile.chromaticAberration.settings = chromaticAberrationSettings;
        }
        else
        {
            chromaticAberrationSettings.intensity -= startwert * Time.deltaTime / time;
            profile.chromaticAberration.settings = chromaticAberrationSettings;
        }

    }
}

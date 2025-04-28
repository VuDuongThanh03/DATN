using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using static SoundDataScriptableObject;

public class SoundManager : Singleton<SoundManager>
{
    public SoundDataScriptableObject soundResource;

    [SerializeField][Range(0, 1f)] private float soundVolume = 1.0f;
    [SerializeField][Range(0, 1f)] private float musicVolume = 1.0f;

    [Header("Sounds")]
    [SerializeField] private Transform SoundSourceContainer;
    [SerializeField] private int SoundSourceAmount = 5;


    [Header("Music")]
    [SerializeField] private AudioSource MusicFXSource;

    [Space(10)]
    [Tooltip("Using effect FadeIn and FadeOut for Playing Music backgroud")]
    public bool FadeInFadeOutBGM = true;

    [Range(0.1f, 1)]
    public float FadeOutTime = 0.25f;
    [Range(0.1f, 1)]
    public float FadeInTime = 0.5f;

    #region Settings
    public bool IsSoundFXEnable
    {
        get
        {
            if (PlayerPrefs.HasKey("IsSound"))
            {
                return PlayerPrefs.GetInt("IsSound") == 1;
            }
            else
            {
                PlayerPrefs.SetInt("IsSound", 1);
                return true;
            }
        }

        set
        {
            PlayerPrefs.SetInt("IsSound", value ? 1 : 0);
        }
    }

    public bool IsMusicEnable
    {
        get
        {
            if (PlayerPrefs.HasKey("IsMusic"))
            {
                return PlayerPrefs.GetInt("IsMusic") == 1;
            }
            else
            {
                PlayerPrefs.SetInt("IsMusic", 1);
                return true;
            }
        }

        set
        {
            PlayerPrefs.SetInt("IsMusic", value ? 1 : 0);

            if (value == false && MusicFXSource.isPlaying)
            {
                PauseMusic();
            }
            else
            {
                if (MusicFXSource.clip != null)
                {
                    ResumeMusic();
                }else
                {
                    PlayMusic(SoundMusicID.BGM_Main);
                }
            }
        }
    }


    #endregion

    #region SoundFX
    private List<AudioSource> audioSources = new List<AudioSource>();

    private Dictionary<Object, List<AudioSource>> audioSourcesDict = new Dictionary<Object, List<AudioSource>>();

    protected override void Awake()
    {
        for (int i = 0; i < SoundSourceAmount; i++)
        {
            AudioSource audioSource = this.SoundSourceContainer.gameObject.AddComponent<AudioSource>();
            SetSourceDefaultSettings(audioSource);
            audioSources.Add(audioSource);
        }

        if (MusicFXSource == null)
        {
            MusicFXSource = this.gameObject.AddComponent<AudioSource>();
            SetSourceDefaultSettings(MusicFXSource, SoundType.SOUND_MUSIC);
        }
        else
        {
            SetSourceDefaultSettings(MusicFXSource, SoundType.SOUND_MUSIC);
        }

        soundResource.Init();

        base.Awake();
    }
    void Start()
    {
        PlayMusic(SoundMusicID.BGM_Main);
    }

    public void PlaySoundFX(SoundFXID soundFX)
    {
        if (IsSoundFXEnable == false)
        {
            return;
        }

        var audioClip = soundResource.GetSoundFXAudioClip(soundFX);
        if (audioClip != null)
        {
            AudioSource audioSource = GetSoundFXAudioSource();
            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("SoundFXID: " + soundFX + " not found in SoundDataScriptableObject");
        }
    }

    public void PlaySoundFX(SoundFXID soundFX, float volume)
    {
        if (IsSoundFXEnable == false)
        {
            return;
        }

        var audioClip = soundResource.GetSoundFXAudioClip(soundFX);

        if (audioClip != null)
        {
            AudioSource audioSource = GetSoundFXAudioSource();
            audioSource.volume = volume;
            audioSource.PlayOneShot(audioClip, volume);
        }
        else
        {
            Debug.LogError("SoundFXID: " + soundFX + " not found in SoundDataScriptableObject");
        }
    }

    public AudioSource PlaySoundFXLoop(SoundFXID soundFX)
    {
        if (IsSoundFXEnable == false)
        {
            return null;
        }

        var audioClip = soundResource.GetSoundFXAudioClip(soundFX);

        if (audioClip != null)
        {
            AudioSource audioSource = GetSoundFXAudioSource();
            audioSource.volume = soundVolume;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();

            return audioSource;
        }
        else
        {
            Debug.LogError("SoundFXID: " + soundFX + " not found in SoundDataScriptableObject");

            return null;
        }
    }

    public AudioSource PlaySoundFXLoop(SoundFXID soundFX, float volume)
    {
        if (IsSoundFXEnable == false)
        {
            return null;
        }

        var audioClip = soundResource.GetSoundFXAudioClip(soundFX);

        if (audioClip != null)
        {
            AudioSource audioSource = GetSoundFXAudioSource();
            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();

            return audioSource;
        }
        else
        {
            Debug.LogError("SoundFXID: " + soundFX + " not found in SoundDataScriptableObject");

            return null;
        }
    }

    public void StopSoundFXLoop(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
        }
    }

    private AudioSource GetSoundFXAudioSource()
    {
        int sourcesAmount = audioSources.Count;
        for (int i = 0; i < sourcesAmount; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }

        AudioSource createdSource = CreateSoundFXAudioSourceObject();
        audioSources.Add(createdSource);

        return createdSource;
    }

    private AudioSource CreateSoundFXAudioSourceObject()
    {
        AudioSource audioSource = this.SoundSourceContainer.gameObject.AddComponent<AudioSource>();
        SetSourceDefaultSettings(audioSource);

        return audioSource;
    }

    public void SetSourceDefaultSettings(AudioSource source, SoundType type = SoundType.SOUND_FX)
    {
        if (type == SoundType.SOUND_FX)
        {
            source.loop = false;
            source.volume = soundVolume;
        }
        else if (type == SoundType.SOUND_MUSIC)
        {
            source.loop = true;
            source.volume = musicVolume;
        }

        source.clip = null;
        source.pitch = 1.0f;
        source.spatialBlend = 0; // 2D Sound
        source.mute = false;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = null;
    }

    #endregion

    #region Music

    public void PlayMusic(SoundMusicID soundMusic)
    {
        if(IsMusicEnable == false)
        {
            return;
        }

        var audioClip = soundResource.GetSoundMusicAudioClip(soundMusic);
        if (audioClip != null)
        {
            if (FadeInFadeOutBGM)
            {
                FadeInPlayMusic(audioClip, FadeOutTime, FadeInTime);
            }
            else
            {
                MusicFXSource.clip = audioClip;
                MusicFXSource.Play();
            }
        }
        else
        {
            Debug.LogError("SoundMusicID: " + soundMusic + " not found in SoundDataScriptableObject");
        }
    }

    public void PauseMusic()
    {
        MusicFXSource.Pause();
    }

    public void ResumeMusic()
    {
        fadeInCoroutine = StartCoroutine(Fadein(0.5f, MusicFXSource.clip));
    }

    public void StopMusic()
    {
        MusicFXSource.Stop();
    }

    #endregion 

    #region Volunme

    public void SetSoundVolume(float volume)
    {
        soundVolume = Mathf.Clamp01(volume);
        foreach (var source in audioSources)
        {
            source.volume = soundVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        MusicFXSource.volume = musicVolume;
    }

    #endregion

    #region Utils
    //Caching
    private Coroutine fadeOutCoroutine = null;
    private Coroutine fadeInCoroutine = null;
    private static float timeStep = 0.01f;
    private WaitForSeconds waitTimeStep = new WaitForSeconds(timeStep);

    private void FadeInPlayMusic(AudioClip fadeinAudioClip, float fadeoutTime, float fadeinTime)
    {
        if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
        fadeOutCoroutine = StartCoroutine(Fadeout(fadeoutTime, () =>
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = StartCoroutine(Fadein(fadeinTime, fadeinAudioClip));
        }));
    }

    IEnumerator Fadeout(float fadeoutTime, UnityAction callback)
    {
        if (MusicFXSource.isPlaying == false || fadeoutTime <= 0)
            callback.Invoke();
        else
        {
            float _time = fadeoutTime;
            while (_time >= 0)
            {
                yield return waitTimeStep;
                _time -= timeStep;
                MusicFXSource.volume = Mathf.Clamp01(_time / fadeoutTime);
            }

            MusicFXSource.volume = 0;
            callback.Invoke();
        }
    }

    IEnumerator Fadein(float fadeinTime, AudioClip audioClip)
    {
        MusicFXSource.clip = audioClip;
        MusicFXSource.volume = 0;

        MusicFXSource.Play();

        if (fadeinTime <= 0)
        {
            MusicFXSource.volume = 1;
            yield break;
        }

        float _time = 0;
        while (_time <= fadeinTime)
        {
            yield return waitTimeStep;
            _time += timeStep;
            MusicFXSource.volume = Mathf.Clamp01(_time / fadeinTime);
        }
        MusicFXSource.volume = 1;
    }


    #endregion
}

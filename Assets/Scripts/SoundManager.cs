using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sound Effects")]
    public AudioSource sfxSource;
    public List<AudioClip> sfxClips;

    [Header("Background Music")]
    public AudioSource bgmSource;
    public List<AudioClip> bgmClips;

    private Dictionary<string, AudioClip> sfxDictionary;
    private Dictionary<string, AudioClip> bgmDictionary;


    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        AddClips();
    }

    private void Start()
    {
        PlayBGM("energy_background_music_1");
    }
    public void AddClips()
    {
        sfxDictionary = new Dictionary<string, AudioClip>();
        bgmDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in sfxClips)
        {
            sfxDictionary.Add(clip.name, clip);
        }

        foreach (AudioClip clip in bgmClips)
        {
            bgmDictionary.Add(clip.name, clip);
        }
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDictionary.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(sfxDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning("SoundManager: Sound effect not found - " + clipName);
        }
    }

    public void PlayBGM(string clipName)
    {
        if (bgmDictionary.ContainsKey(clipName))
        {
            bgmSource.clip = bgmDictionary[clipName];
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("SoundManager: Background music not found - " + clipName);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetVolume(int target)
    {
        sfxSource.volume = 0;
        bgmSource.volume = 0;
        if (target == 0)
        {
            sfxSource.volume = 1;
            bgmSource.volume =.6f;
        }
        else if(target == 1)
        {
            sfxSource.volume = 1;
        }
        else if (target == 2)
        {
            bgmSource.volume = .6f;
        } else 
        {
            sfxSource.volume = 0;
            bgmSource.volume = 0;
        }
       
    }
}

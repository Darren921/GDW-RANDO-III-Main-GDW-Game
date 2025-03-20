using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, singleSounds, monsterSounds, monsterSingleSounds;
    public AudioSource musicSource, sfxSource, singleSource, monsterSource, monsterSingleSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }

    public void StopSFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
//            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Stop();
        }
    }

    public void PlayMonsterSFX(string name)
    {
        Sound s = Array.Find(monsterSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            monsterSource.clip = s.clip;
            monsterSource.Play();
        }
    }

    public void StopMonsterSFX(string name)
    {
        Sound s = Array.Find(monsterSounds, x => x.name == name);

        if (s == null)
        {
            //Debug.Log("Sound Not Found");
        }

        else
        {
            monsterSource.clip = s.clip;
            monsterSource.Stop();
        }
    }

    public void PlayMonsterSingleSFX(string name)
    {
        Sound s = Array.Find(monsterSingleSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            monsterSingleSource.clip = s.clip;
            monsterSingleSource.Play();
        }
    }

    public void PlaySingleSFX(string name)
    {
        Sound s = Array.Find(singleSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            singleSource.clip = s.clip;
            singleSource.Play();
        }
    }

}

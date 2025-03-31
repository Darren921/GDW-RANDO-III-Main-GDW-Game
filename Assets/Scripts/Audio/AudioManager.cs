using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, footstepSounds, spotCueSounds, flashlightSounds, torchSounds, monsterFootstepSounds, monsterSingleSounds;
    public AudioSource musicSource, footstepSource, spotCueSource, flashlightSource, torchSource, monsterFootstepSource, monsterSingleSource;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindNewMonsterSource();
    }

    private void FindNewMonsterSource()
    {
        MonsterAIPathfinding newMonster = FindObjectOfType<MonsterAIPathfinding>();

        if (newMonster != null)
        {
            AudioSource[] sources = newMonster.GetComponentsInChildren<AudioSource>();

            if (sources.Length > 0)
            {
                monsterFootstepSource = sources[0];

                if (sources.Length > 1)
                {
                    monsterSingleSource = sources[1];
                }
            }
        }
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

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(footstepSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            footstepSource.clip = s.clip;
            footstepSource.Play();
        }
    }

    public void StopSFX(string name)
    {
        Sound s = Array.Find(footstepSounds, x => x.name == name);

        if (s == null)
        {
//            Debug.Log("Sound Not Found");
        }

        else
        {
            footstepSource.clip = s.clip;
            footstepSource.Stop();
        }
    }

    public void PlayMonsterFootstepSFX(string name)
    {
        Sound s = Array.Find(monsterFootstepSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            monsterFootstepSource.clip = s.clip;
            monsterFootstepSource.Play();
        }
    }

    public void StopMonsterFootstepSFX(string name)
    {
        Sound s = Array.Find(monsterFootstepSounds, x => x.name == name);

        if (s == null)
        {
            //Debug.Log("Sound Not Found");
        }

        else
        {
            monsterFootstepSource.clip = s.clip;
            monsterFootstepSource.Stop();
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

    public void PlaySpotCueSFX(string name)
    {
        Sound s = Array.Find(spotCueSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            spotCueSource.clip = s.clip;
            spotCueSource.Play();
        }
    }

    public void PlayFlashlightSFX(string name)
    {
        Sound s = Array.Find(flashlightSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            flashlightSource.clip = s.clip;
            flashlightSource.Play();
        }
    }

    public void PlayTorchSFX(string name)
    {
        Sound s = Array.Find(torchSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            torchSource.clip = s.clip;
            torchSource.Play();
        }
    }

    public void StopTorchSFX(string name)
    {
        Sound s = Array.Find(torchSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            torchSource.clip = s.clip;
            torchSource.Stop();
        }
    }
}

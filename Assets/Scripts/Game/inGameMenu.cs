using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class inGameMenu : MonoBehaviour
{
    GameObject menu;
    [SerializeField]Slider Sens;
    [SerializeField]Slider Music;
    [SerializeField]Slider SFX;
   
    public static bool MusicState;
    public static bool SFXState;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField]  PlayerMovement player;
    List<AudioSource> sfxSources = new List<AudioSource>();
    List<AudioSource> musicSources = new List<AudioSource>();

    private bool isOpen;
    private void Awake()
    {
        MusicState = true;
        SFXState = true;
        menu = gameObject;
        Sens.minValue = 0.09f;
        Sens.maxValue = 0.1f;
        Music.maxValue = 1;
        SFX.maxValue = 1;
        
        var allSourcesSFX = GameObject.FindGameObjectsWithTag("SFX");
        for (var i = 0; i < allSourcesSFX.Length; i++)
        {
            sfxSources.Add(allSourcesSFX[i].GetComponent<AudioSource>());
        }
        var allSourcesMusic = GameObject.FindGameObjectsWithTag("Music");
        for (var i = 0; i < allSourcesMusic.Length; i++)
        {
            musicSources.Add(allSourcesMusic[i].GetComponent<AudioSource>());
        }
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.volume = SFX.value;
        }
        foreach (var musicSource in musicSources)
        {
            musicSource.volume = Music.value;
        }
        
//        print(Sens.value);
        Sens.value = _camera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
        Sens.value = _camera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
        
      
    }

    void Start()
    {
        menu.SetActive(false);
    }

    
    public void Modify()
    {
        _camera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = Sens.value;
        _camera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = Sens.value;
    }
    public void ModifySFX()
    {
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.volume = SFX.value;
        }
    }
    public void ModifyMusic()
    {
        foreach (var musicSource in musicSources)
        {
            musicSource.volume = Music.value;
        }
    }
    
    public void OpenMenu()
    {
        if (!isOpen)
        {
            isOpen = true;
            player.DisableInput();
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            menu.SetActive(true);
        }
        else
        {
            player.EnableInput();
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            menu.SetActive(false);
            isOpen = false;
        }
      
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }

  
}